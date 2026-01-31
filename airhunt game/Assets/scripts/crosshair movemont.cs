using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class CrosshairFromPython : MonoBehaviour
{
    public RectTransform crosshair;

    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;

    float aimX = 0.5f;
    float aimY = 0.5f;

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
                if (parts.Length < 10) continue;

                // landmark 8 = index finger tip
                string[] xy = parts[9].Split(',');

                float.TryParse(xy[0], NumberStyles.Float, CultureInfo.InvariantCulture, out aimX);
                float.TryParse(xy[1], NumberStyles.Float, CultureInfo.InvariantCulture, out aimY);
            }
            catch
            {
                break;
            }
        }
    }

    void Update()
    {
        crosshair.anchoredPosition = new Vector2(
            (aimX - 0.5f) * Screen.width,
            (0.5f - aimY) * Screen.height
        );
    }

    void OnApplicationQuit()
    {
        receiveThread?.Abort();
        stream?.Close();
        client?.Close();
    }
}
