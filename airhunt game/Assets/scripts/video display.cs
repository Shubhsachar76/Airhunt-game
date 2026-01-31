using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System.Threading;

public class PythonVideoReceiver : MonoBehaviour
{
    public RawImage rawImage;

    TcpClient client;
    NetworkStream stream;
    Thread thread;
    Texture2D tex;

    void Start()
    {
        tex = new Texture2D(2, 2);
        client = new TcpClient("127.0.0.1", 9998);
        stream = client.GetStream();

        thread = new Thread(Receive);
        thread.IsBackground = true;
        thread.Start();
    }

    void Receive()
    {
        BinaryReader reader = new BinaryReader(stream);

        while (true)
        {
            try
            {
                int size = System.Net.IPAddress.NetworkToHostOrder(reader.ReadInt32());
                if (size <= 0 || size > 5_000_000) continue;

                byte[] data = reader.ReadBytes(size);
                lock (this)
                {
                    tex.LoadImage(data);
                }
            }
            catch
            {
                break;
            }
        }
    }

    void Update()
    {
        lock (this)
        {
            if (tex != null)
                rawImage.texture = tex;
        }
    }

    void OnApplicationQuit()
    {
        thread?.Abort();
        stream?.Close();
        client?.Close();
    }
}
