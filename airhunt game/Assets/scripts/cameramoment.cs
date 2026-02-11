using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class CameraFromPython : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;

    float aimX = 0.5f;
    float aimY = 0.5f;

    float startX;
    float startY;

    bool calibrated = false;
    bool handDetected = false;

    int shootFlag = 0;
    int lastShootFlag = 0;

    [Header("Sensitivity Settings")]
    public float horizontalSensitivity = 400f;
    public float verticalSensitivity = 350f;
    public float deadzone = 0.01f;

    [Header("Shooting")]
    public float range = 100f;
    public int damage = 25;

    void Start()
    {
        client = new TcpClient("127.0.0.1", 9999);
        stream = client.GetStream();

        receiveThread = new Thread(Receive);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Receive()
    {
        byte[] buffer = new byte[8192];

        while (true)
        {
            try
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string msg = Encoding.ASCII.GetString(buffer, 0, bytes);

                if (!msg.StartsWith("HAND")) continue;

                string[] parts = msg.Split(' ');
                if (parts.Length < 23) continue;

                // Landmark 8 (index tip slot)
                string[] xy = parts[9].Split(',');

                float.TryParse(xy[0], NumberStyles.Float, CultureInfo.InvariantCulture, out aimX);
                float.TryParse(xy[1], NumberStyles.Float, CultureInfo.InvariantCulture, out aimY);

                int.TryParse(parts[parts.Length - 1], out shootFlag);

                handDetected = true;
            }
            catch
            {
                break;
            }
        }
    }

    void Update()
    {
        if (!handDetected) return;

        // Calibrate once when hand first appears
        if (!calibrated)
        {
            startX = aimX;
            startY = aimY;
            calibrated = true;
        }

        float deltaX = aimX - startX;
        float deltaY = aimY - startY;

        // Deadzone to remove micro shaking
        if (Mathf.Abs(deltaX) < deadzone) deltaX = 0f;
        if (Mathf.Abs(deltaY) < deadzone) deltaY = 0f;

        float yaw = deltaX * horizontalSensitivity;
        float pitch = -deltaY * verticalSensitivity;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Shoot on fist close (0 → 1 transition)
        if (shootFlag == 1 && lastShootFlag == 0)
        {
            Shoot();
        }

        lastShootFlag = shootFlag;
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void OnApplicationQuit()
    {
        receiveThread?.Abort();
        stream?.Close();
        client?.Close();
    }
}
