using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class CanvasScreen: MonoBehaviour
{
    [System.Serializable]
    public class ScreenData
    {
        [Tooltip("Toda tela deve ter um nome que possa ser chamada")]
        public string screenName;
        public string previusScreenName;
        public string nextScreenName;
        [Header("- editor -")]
        public bool editor_turnOn = false;
        public bool editor_turnOff = false;
    }
    [Tooltip("Toda tela deve ter uma base de canvas group")]
    public CanvasGroup canvasgroup;
    [SerializeField] protected ScreenData data;
    public ScreenData screenData;
    public virtual void OnValidate()
    {
        if (canvasgroup == null)
        {
            canvasgroup = GetComponent<CanvasGroup>();
        }
        
        screenData = data;

        if (data.editor_turnOff)
        {
            data.editor_turnOff = false;

            if (canvasgroup != null)
            {
                TurnOff();
            }
            else
            {
                Debug.LogError("CanvasGroup está nulo ao tentar desativar no OnValidate.", this);
            }
        }

        if (data.editor_turnOn)
        {
            data.editor_turnOn = false;

            foreach (var screen in FindObjectsByType<CanvasScreen>(FindObjectsSortMode.None))
            {
                if (screen != this && screen.canvasgroup != null)
                {
                    screen.TurnOff();
                }
            }

            if (canvasgroup != null)
            {
                TurnOn();
            }
            else
            {
                Debug.LogError("CanvasGroup está nulo ao tentar ativar no OnValidate.", this);
            }
        }
    }

    public virtual void Awake()
    {
        if (canvasgroup == null)
        {
            canvasgroup = GetComponent<CanvasGroup>();
        }
        
        if (screenData != data)
        {
            Debug.LogWarning($"[CanvasScreen] screenData and data are different references on {gameObject.name}. Syncing screenData to data.", this);
            screenData = data;
        }
        
        if (string.IsNullOrEmpty(data.screenName))
        {
            Debug.LogError($"[CanvasScreen] CRITICAL - screenName is NULL or EMPTY on GameObject: {gameObject.name}! This will cause screen system to fail!", this);
        }
        
        Debug.Log($"[CanvasScreen] '{data.screenName}' Awake - GameObject: {gameObject.name}, Active: {gameObject.activeSelf}");
        ScreenManager.CallScreen += CallScreenListner;
        Debug.Log($"[CanvasScreen] '{data.screenName}' registered to ScreenManager.CallScreen event");
    }
    
    public virtual void OnEnable()
    {
        Debug.Log($"[CanvasScreen] '{data.screenName}' OnEnable - GameObject enabled");
    }
    
    public virtual void OnDisable()
    {
        Debug.Log($"[CanvasScreen] '{data.screenName}' OnDisable - GameObject disabled");
    }
    
    public virtual void OnDestroy()
    {
        Debug.Log($"[CanvasScreen] '{data.screenName}' OnDestroy - Unregistering from event");
        ScreenManager.CallScreen -= CallScreenListner;
    }

    public virtual void CallScreenListner(string screenName)
    {
        if (string.IsNullOrEmpty(this.data.screenName))
        {
            Debug.LogError($"[CanvasScreen] CRITICAL - this.data.screenName is NULL or EMPTY on GameObject: {gameObject.name}!", this);
            return;
        }
        
        if (string.IsNullOrEmpty(screenName))
        {
            Debug.LogError($"[CanvasScreen] CRITICAL - Requested screenName is NULL or EMPTY!", this);
            return;
        }
        
        Debug.Log($"[CanvasScreen] '{data.screenName}' CallScreenListner - Requested: '{screenName}', Match: {screenName == this.data.screenName}");
        
        if (screenName == this.data.screenName)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
    
    public virtual void TurnOn()
    {
        Debug.Log($"[CanvasScreen] '{data.screenName}' TurnOn - GameObject: {gameObject.name}");
        canvasgroup.alpha = 1;
        canvasgroup.interactable = true;
        canvasgroup.blocksRaycasts = true;
        gameObject.SetActive(true);
    }
    
    public virtual void TurnOff()
    {
        Debug.Log($"[CanvasScreen] '{data.screenName}' TurnOff - GameObject: {gameObject.name}");
        Debug.Log($"[CanvasScreen] TurnOff called from:\n{System.Environment.StackTrace}");
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
    public bool IsOn()
    {
        return canvasgroup.blocksRaycasts;
    }

    public virtual void CallNextScreen()
    {
        ScreenManager.CallScreen(data.nextScreenName);
    }
    public virtual void CallPreviusScreen()
    {
        ScreenManager.CallScreen(data.previusScreenName);
    }

    public virtual void CallScreenByName(string _name)
    {
        ScreenManager.CallScreen(_name);
    }
}