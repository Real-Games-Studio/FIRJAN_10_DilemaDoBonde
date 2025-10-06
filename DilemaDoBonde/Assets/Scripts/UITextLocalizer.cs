using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Component responsible for localizing static UI text elements
/// </summary>
public class UITextLocalizer : MonoBehaviour
{
    [Header("Localization Settings")]
    [SerializeField] private string textId;
    
    private Text legacyText;
    private TextMeshProUGUI tmpText;
    
    void Awake()
    {
        // Cache text component references
        legacyText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();
        
        // Subscribe to language change events
        LanguageManager.OnLanguageChanged += UpdateText;
    }
    
    void Start()
    {
        UpdateText();
    }
    
    void OnDestroy()
    {
        LanguageManager.OnLanguageChanged -= UpdateText;
    }
    
    /// <summary>
    /// Sets the text ID for localization
    /// </summary>
    public void SetTextId(string id)
    {
        textId = id;
        UpdateText();
    }
    
    /// <summary>
    /// Updates the displayed text based on current language
    /// </summary>
    private void UpdateText()
    {
        if (string.IsNullOrEmpty(textId)) return;
        
        // Check if we're in play mode and LanguageManager is available
        if (!Application.isPlaying || LanguageManager.Instance == null)
        {
            return;
        }
        
        string localizedText = StaticTextManager.GetText(textId);
        
        if (legacyText != null)
        {
            legacyText.text = localizedText;
        }
        else if (tmpText != null)
        {
            tmpText.text = localizedText;
        }
    }
    
    void OnValidate()
    {
        // Only update text in play mode when everything is properly initialized
        if (Application.isPlaying && LanguageManager.Instance != null)
        {
            UpdateText();
        }
    }
}