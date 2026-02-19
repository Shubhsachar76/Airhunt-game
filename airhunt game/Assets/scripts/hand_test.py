import cv2
import mediapipe as mp
import socket
import math
import time
#python Assets/scripts/hand_test.py

mp_hands = mp.solutions.hands

hands = mp_hands.Hands(
    max_num_hands=1,
    min_detection_confidence=0.6,
    min_tracking_confidence=0.6
)

cap = cv2.VideoCapture(0)

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server.bind(("127.0.0.1", 9999))
server.listen(1)

print("Python server started")

def dist(a, b):
    return math.hypot(a.x - b.x, a.y - b.y)

def finger_extended(tip, pip, mcp):
    return dist(tip, pip) > dist(pip, mcp)

def finger_folded(tip, pip, mcp):
    return dist(tip, pip) < dist(pip, mcp)

def is_open(lm):
    return all([
        finger_extended(lm[8], lm[6], lm[5]),
        finger_extended(lm[12], lm[10], lm[9]),
        finger_extended(lm[16], lm[14], lm[13]),
        finger_extended(lm[20], lm[18], lm[17])
    ])

def is_fist(lm):
    return all([
        finger_folded(lm[8], lm[6], lm[5]),
        finger_folded(lm[12], lm[10], lm[9]),
        finger_folded(lm[16], lm[14], lm[13]),
        finger_folded(lm[20], lm[18], lm[17])
    ])

try:
    while True:
        print("Waiting for Unity...")
        conn, _ = server.accept()
        print("Unity connected")

        was_open = False

        try:
            while True:
                ret, frame = cap.read()
                if not ret:
                    break

                frame = cv2.flip(frame, 1)
                rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                result = hands.process(rgb)

                shoot = 0
                landmarks = []

                if result.multi_hand_landmarks:
                    lm = result.multi_hand_landmarks[0].landmark

                    # ---- Use Palm Center (Stable, No Smoothing) ----
                    palm_x = (lm[0].x + lm[5].x + lm[17].x) / 3
                    palm_y = (lm[0].y + lm[5].y + lm[17].y) / 3

                    for i, p in enumerate(lm):
                        if i == 8:
                            # Replace index tip with palm center
                            landmarks.append(f"{palm_x:.3f},{palm_y:.3f}")
                        else:
                            landmarks.append(f"{p.x:.3f},{p.y:.3f}")

                    # ---- Shoot Logic ----
                    open_now = is_open(lm)
                    fist_now = is_fist(lm)

                    if open_now:
                        was_open = True

                    if was_open and fist_now:
                        shoot = 1
                        was_open = False

                else:
                    landmarks = ["0.5,0.5"] * 21

                msg = "HAND " + " ".join(landmarks) + f" {shoot}\n"
                conn.sendall(msg.encode())

                time.sleep(0.01)

        except:
            print("Unity disconnected")

        finally:
            conn.close()

except KeyboardInterrupt:
    print("Shutting down")

finally:
    cap.release()
    server.close()
