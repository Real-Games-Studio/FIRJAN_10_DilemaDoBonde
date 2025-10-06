using UnityEngine;

public enum Language
{
    Portuguese,
    English
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    
    [Header("Language Settings")]
    public Language currentLanguage = Language.Portuguese;
    
    public static System.Action OnLanguageChanged;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StaticTextManager.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            Debug.Log($"Language changed to: {language}");
            OnLanguageChanged?.Invoke();
        }
    }
    
    public void SetPortuguese()
    {
        SetLanguage(Language.Portuguese);
        Debug.Log("adasdassda");
    }
    
    public void SetEnglish()
    {
        SetLanguage(Language.English);
    }
    
    public bool IsPortuguese()
    {
        return currentLanguage == Language.Portuguese;
    }
    
    public bool IsEnglish()
    {
        return currentLanguage == Language.English;
    }
    
    public string GetCurrentLanguageCode()
    {
        return currentLanguage == Language.Portuguese ? "pt" : "en";
    }
}