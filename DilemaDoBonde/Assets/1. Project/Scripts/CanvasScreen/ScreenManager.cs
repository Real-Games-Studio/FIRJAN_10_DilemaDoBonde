using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class ScreenManager
{
    public static Action OnReset;
    public static Action<string> CallScreen;
    public static string currentScreenName;
    public static void SetCallScreen(string name)
    {
        Debug.Log($"[ScreenManager] SetCallScreen called - Screen: '{name}', Listeners: {CallScreen?.GetInvocationList().Length ?? 0}");
        CallScreen?.Invoke(name);
        currentScreenName = name;
        Debug.Log($"[ScreenManager] Current screen set to: '{currentScreenName}'");
    }
    
    public static string GetCurrentScreenName()
    {
        return currentScreenName;
    }

    public static void TurnOnCanvasGroup(CanvasGroup c)
    {
        c.alpha = 1;
        c.interactable = true;
        c.blocksRaycasts = true;
        c.gameObject.SetActive(true);
    }

    public static void TurnOffCanvasGroup(CanvasGroup c)
    {
        c.alpha = 0;
        c.interactable = false;
        c.blocksRaycasts = false;
        c.gameObject.SetActive(false);
    }
}
