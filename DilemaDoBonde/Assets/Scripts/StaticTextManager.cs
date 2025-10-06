using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Manages static UI text localization data
/// </summary>
public static class StaticTextManager
{
    private static Dictionary<string, StaticTextData> textDatabase;
    private static bool isInitialized = false;
    
    private const string STATIC_TEXTS_FILENAME = "StaticTexts.json";
    
    /// <summary>
    /// Initializes the static text manager by loading the JSON data
    /// </summary>
    public static void Initialize()
    {
        if (isInitialized) return;
        
        LoadStaticTexts();
        isInitialized = true;
    }
    
    /// <summary>
    /// Gets localized text by ID for the current language
    /// </summary>
    public static string GetText(string textId)
    {
        if (!isInitialized)
        {
            Initialize();
        }
        
        if (textDatabase == null || !textDatabase.ContainsKey(textId))
        {
            Debug.LogWarning($"Static text with ID '{textId}' not found in database");
            return $"[MISSING: {textId}]";
        }
        
        StaticTextData textData = textDatabase[textId];
        
        // Safety check for LanguageManager instance
        if (LanguageManager.Instance == null)
        {
            Debug.LogWarning("LanguageManager.Instance is null, defaulting to Portuguese");
            return textData.textPt;
        }
        
        return LanguageManager.Instance.IsPortuguese() ? textData.textPt : textData.textEn;
    }
    
    /// <summary>
    /// Loads static texts from JSON file in StreamingAssets
    /// </summary>
    private static void LoadStaticTexts()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, STATIC_TEXTS_FILENAME);
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Static texts JSON file not found at path: {filePath}");
            textDatabase = new Dictionary<string, StaticTextData>();
            return;
        }
        
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            StaticTextDatabase database = JsonConvert.DeserializeObject<StaticTextDatabase>(jsonContent);
            textDatabase = new Dictionary<string, StaticTextData>();
            
            foreach (var text in database.staticTexts)
            {
                if (!textDatabase.ContainsKey(text.id))
                {
                    textDatabase.Add(text.id, text);
                }
                else
                {
                    Debug.LogWarning($"Duplicate text ID found: {text.id}");
                }
            }
            
            Debug.Log($"Loaded {textDatabase.Count} static texts from StreamingAssets");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading static texts JSON: {e.Message}");
            textDatabase = new Dictionary<string, StaticTextData>();
        }
    }
    
    /// <summary>
    /// Reloads the static texts from JSON (useful for development)
    /// </summary>
    public static void ReloadTexts()
    {
        isInitialized = false;
        Initialize();
    }
    
    /// <summary>
    /// Gets all available text IDs
    /// </summary>
    public static string[] GetAllTextIds()
    {
        if (!isInitialized)
        {
            Initialize();
        }
        
        if (textDatabase == null) return new string[0];
        
        string[] ids = new string[textDatabase.Count];
        textDatabase.Keys.CopyTo(ids, 0);
        return ids;
    }
}

/// <summary>
/// Data structure for individual static text entries
/// </summary>
[System.Serializable]
public class StaticTextData
{
    public string id;
    public string textPt;
    public string textEn;
}

/// <summary>
/// Data structure for the complete static text database
/// </summary>
[System.Serializable]
public class StaticTextDatabase
{
    public StaticTextData[] staticTexts;
}