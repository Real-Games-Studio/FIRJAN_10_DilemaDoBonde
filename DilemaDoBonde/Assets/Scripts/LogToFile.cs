using UnityEngine;
using System.IO;
using System;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logWriter;

    void Awake()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(Application.persistentDataPath, $"ScreenDebug_{timestamp}.log");
        
        try
        {
            logWriter = new StreamWriter(logFilePath, true);
            logWriter.AutoFlush = true;
            
            Application.logMessageReceived += HandleLog;
            
            Debug.Log($"[LogToFile] Logging to: {logFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[LogToFile] Failed to create log file: {e.Message}");
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logWriter != null)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            logWriter.WriteLine($"[{timestamp}] [{type}] {logString}");
            
            if (type == LogType.Error || type == LogType.Exception)
            {
                logWriter.WriteLine(stackTrace);
            }
        }
    }

    void OnDestroy()
    {
        if (logWriter != null)
        {
            Application.logMessageReceived -= HandleLog;
            logWriter.Close();
            Debug.Log($"[LogToFile] Log file saved to: {logFilePath}");
        }
    }

    void OnApplicationQuit()
    {
        OnDestroy();
    }
}
