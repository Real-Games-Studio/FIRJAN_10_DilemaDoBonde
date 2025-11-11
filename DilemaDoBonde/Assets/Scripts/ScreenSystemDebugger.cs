using UnityEngine;
using System.Linq;

public class ScreenSystemDebugger : MonoBehaviour
{
    private const float LOG_INTERVAL = 2f;
    private float nextLogTime = 0f;

    void Start()
    {
        Debug.Log("====================================");
        Debug.Log("[ScreenSystemDebugger] SYSTEM INITIALIZED");
        Debug.Log($"[ScreenSystemDebugger] Platform: {Application.platform}");
        Debug.Log($"[ScreenSystemDebugger] Is Editor: {Application.isEditor}");
        Debug.Log($"[ScreenSystemDebugger] Build GUID: {Application.buildGUID}");
        Debug.Log("====================================");
        
        LogAllScreensState();
    }

    void Update()
    {
        if (Time.time >= nextLogTime)
        {
            nextLogTime = Time.time + LOG_INTERVAL;
            LogAllScreensState();
        }
    }

    void LogAllScreensState()
    {
        CanvasScreen[] allScreens = Object.FindObjectsByType<CanvasScreen>(FindObjectsSortMode.None);
        
        Debug.Log($"[ScreenSystemDebugger] === PERIODIC STATE CHECK (Time: {Time.time:F2}s) ===");
        Debug.Log($"[ScreenSystemDebugger] Total CanvasScreen components found: {allScreens.Length}");
        
        if (ScreenCanvasController.instance != null)
        {
            Debug.Log($"[ScreenSystemDebugger] Controller current screen: '{ScreenCanvasController.instance.currentScreen}'");
            Debug.Log($"[ScreenSystemDebugger] Controller initial screen: '{ScreenCanvasController.instance.inicialScreen}'");
            Debug.Log($"[ScreenSystemDebugger] Controller inactive timer: {ScreenCanvasController.instance.inactiveTimer:F2}s");
        }
        else
        {
            Debug.LogError("[ScreenSystemDebugger] ScreenCanvasController.instance is NULL!");
        }
        
        Debug.Log($"[ScreenSystemDebugger] ScreenManager current screen: '{ScreenManager.GetCurrentScreenName()}'");
        
        foreach (var screen in allScreens)
        {
            if (screen != null && screen.gameObject != null)
            {
                bool isActive = screen.gameObject.activeSelf;
                bool isOn = screen.IsOn();
                float alpha = screen.canvasgroup != null ? screen.canvasgroup.alpha : -1f;
                
                Debug.Log($"[ScreenSystemDebugger] Screen: '{screen.screenData.screenName}' | " +
                         $"GameObject: '{screen.gameObject.name}' | " +
                         $"Active: {isActive} | " +
                         $"IsOn: {isOn} | " +
                         $"Alpha: {alpha:F2}");
            }
        }
        
        Debug.Log($"[ScreenSystemDebugger] === END STATE CHECK ===");
    }
}
