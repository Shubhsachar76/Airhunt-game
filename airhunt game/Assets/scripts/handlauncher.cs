using UnityEngine;
using System.Diagnostics;

public class HandLauncher : MonoBehaviour
{
    private Process pythonProcess;

    void Awake()
    {
        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName = "python";
        pythonProcess.StartInfo.Arguments = "Assets/scripts/hand_test.py";
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.CreateNoWindow = true;
        pythonProcess.Start();

        System.Threading.Thread.Sleep(1500);
    }

    void OnApplicationQuit()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
            pythonProcess.Kill();
    }
}