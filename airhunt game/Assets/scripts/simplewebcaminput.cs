using UnityEngine;
using UnityEngine.UI;

public class SimpleHandInput : MonoBehaviour
{
    [Header("References")]
    public RawImage webcamImage;          // RawImage showing webcam
    public RectTransform crosshair;        // UI crosshair
    public RectTransform debugDot;         // Shows input source

    [Header("Settings")]
    public float smoothSpeed = 8f;
    [Range(0f, 1f)]
    public float motionThreshold = 0.2f;  // Increase if head still dominates

    Color32[] prevPixels;
    Color32[] currPixels;

    int width;
    int height;

    void Update()
    {
        if (WebcamDisplay.camTex == null) return;
        if (!WebcamDisplay.camTex.isPlaying || WebcamDisplay.camTex.width < 16) return;

        width = WebcamDisplay.camTex.width;
        height = WebcamDisplay.camTex.height;

        currPixels = WebcamDisplay.camTex.GetPixels32();

        if (prevPixels == null)
        {
            prevPixels = currPixels;
            return;
        }

        float sumX = 0f;
        float sumY = 0f;
        int count = 0;
        int step = 25;

        for (int i = 0; i < currPixels.Length; i += step)
        {
            int x = i % width;
            int y = i / width;

            // Ignore upper part of frame (kills head tracking)
            if (y < height * 0.45f) continue;

            Color32 c = currPixels[i];
            Color32 p = prevPixels[i];

            float diff =
                Mathf.Abs(c.r - p.r) +
                Mathf.Abs(c.g - p.g) +
                Mathf.Abs(c.b - p.b);

            diff /= (3f * 255f);

            if (diff > motionThreshold)
            {
                sumX += x;
                sumY += y;
                count++;
            }
        }

        prevPixels = currPixels;
        if (count == 0) return;

        float avgX = sumX / count;
        float avgY = sumY / count;

        float normX = avgX / width;
        float normY = avgY / height;

        // Map to webcam UI space
        RectTransform webcamRect = webcamImage.rectTransform;
        Rect rect = webcamRect.rect;

        float uiX = (normX - 0.5f) * rect.width;
        float uiY = (0.5f - normY) * rect.height;

        Vector2 uiPos = new Vector2(uiX, uiY);

        // Debug dot = raw input
        debugDot.anchoredPosition = uiPos;

        // Crosshair = smoothed
        crosshair.anchoredPosition = Vector2.Lerp(
            crosshair.anchoredPosition,
            uiPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
