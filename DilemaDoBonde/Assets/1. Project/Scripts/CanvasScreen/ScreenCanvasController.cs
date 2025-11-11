using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RealGames;
using System.IO;

public class ScreenCanvasController : MonoBehaviour
{
    public static ScreenCanvasController instance;
    public AppConfig appConfig;
    public DilemmaConfig dilemmaConfig;

    public string previusScreen;
    public string currentScreen;
    public string inicialScreen;
    public float inactiveTimer = 0;

    public CanvasGroup DEBUG_CANVAS;
    public TMP_Text timeOut;

    private void OnEnable()
    {
        Debug.Log("[ScreenCanvasController] OnEnable - Registering screen call listener");
        ScreenManager.CallScreen += OnScreenCall;
    }
    private void OnDisable()
    {
        Debug.Log("[ScreenCanvasController] OnDisable - Unregistering screen call listener");
        ScreenManager.CallScreen -= OnScreenCall;
    }
    void Start()
    {
        Debug.Log("[ScreenCanvasController] Start - Beginning initialization");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        instance = this;
        
        Debug.Log($"[ScreenCanvasController] Configured initial screen: '{inicialScreen}'");
        
        CanvasScreen[] allScreens = Object.FindObjectsByType<CanvasScreen>(FindObjectsSortMode.None);
        Debug.Log($"[ScreenCanvasController] Found {allScreens.Length} CanvasScreen components in scene");
        
        bool initialScreenExists = false;
        foreach (var screen in allScreens)
        {
            string screenName = screen.screenData?.screenName ?? "NULL";
            Debug.Log($"[ScreenCanvasController] Available screen: '{screenName}' on GameObject '{screen.gameObject.name}'");
            
            if (screenName == inicialScreen)
            {
                initialScreenExists = true;
            }
        }
        
        if (!initialScreenExists)
        {
            Debug.LogError($"[ScreenCanvasController] CRITICAL ERROR - Initial screen '{inicialScreen}' does NOT EXIST in the scene!", this);
            Debug.LogError($"[ScreenCanvasController] This will cause all screens to be deactivated! Available screens are listed above.", this);
        }
        
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
        Debug.Log($"[ScreenCanvasController] Config path: {configPath}");
        dilemmaConfig = JsonLoader.LoadDilemmaConfig(configPath);
        
        if (dilemmaConfig != null)
        {
            appConfig.maxInactiveTime = dilemmaConfig.timeoutSeconds;
            Debug.Log($"[ScreenCanvasController] Config loaded - Timeout: {appConfig.maxInactiveTime}s");
        }
        else
        {
            Debug.LogWarning("[ScreenCanvasController] DilemmaConfig is NULL - using default timeout");
        }
        
        Debug.Log($"[ScreenCanvasController] Calling initial screen: '{inicialScreen}'");
        ScreenManager.SetCallScreen(inicialScreen);
        Debug.Log("[ScreenCanvasController] Start - Initialization complete");
    }
    // Update is called once per frame
    void Update()
    {
        if (currentScreen != inicialScreen)
        {
            inactiveTimer += Time.deltaTime * 1;

            if (inactiveTimer >= appConfig.maxInactiveTime)
            {
                ResetGame();
            }
        }
        else
        {
            inactiveTimer = 0;
        }
    }
    public void ResetGame()
    {
        Debug.Log($"[ScreenCanvasController] ResetGame - Inactive timeout reached! Timer: {inactiveTimer}s");
        inactiveTimer = 0;
        ScreenManager.CallScreen(inicialScreen);
    }
    public void OnScreenCall(string name)
    {
        Debug.Log($"[ScreenCanvasController] OnScreenCall - Screen requested: '{name}'");
        inactiveTimer = 0;
        previusScreen = currentScreen;
        currentScreen = name;
        Debug.Log($"[ScreenCanvasController] Screen transition - Previous: '{previusScreen}', Current: '{currentScreen}'");
    }
    public void NFCInputHandler(string obj)
    {
        inactiveTimer = 0;
    }

    public void CallAnyScreenByName(string name)
    {
        ScreenManager.CallScreen(name);
    }
}
