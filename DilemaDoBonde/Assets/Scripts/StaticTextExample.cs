using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Example script showing how to use the static text system programmatically
/// </summary>
public class StaticTextExample : MonoBehaviour
{
    [Header("UI References")]
    public Text legacyText;
    public TextMeshProUGUI tmpText;
    public Button changeLanguageButton;
    
    void Start()
    {
        // Example of setting text programmatically
        if (legacyText != null)
        {
            legacyText.text = StaticTextManager.GetText("title_main_menu");
        }
        
        if (tmpText != null)
        {
            tmpText.text = StaticTextManager.GetText("btn_start");
        }
        
        // Subscribe to language changes
        LanguageManager.OnLanguageChanged += OnLanguageChanged;
        
        // Setup button to change language
        if (changeLanguageButton != null)
        {
            changeLanguageButton.onClick.AddListener(ToggleLanguage);
        }
    }
    
    void OnDestroy()
    {
        LanguageManager.OnLanguageChanged -= OnLanguageChanged;
    }
    
    /// <summary>
    /// Called when language changes
    /// </summary>
    private void OnLanguageChanged()
    {
        // Update texts when language changes
        if (legacyText != null)
        {
            legacyText.text = StaticTextManager.GetText("title_main_menu");
        }
        
        if (tmpText != null)
        {
            tmpText.text = StaticTextManager.GetText("btn_start");
        }
    }
    
    /// <summary>
    /// Toggles between Portuguese and English
    /// </summary>
    private void ToggleLanguage()
    {
        if (LanguageManager.Instance.IsPortuguese())
        {
            LanguageManager.Instance.SetEnglish();
        }
        else
        {
            LanguageManager.Instance.SetPortuguese();
        }
    }
}