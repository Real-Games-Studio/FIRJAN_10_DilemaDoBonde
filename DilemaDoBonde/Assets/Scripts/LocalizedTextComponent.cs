using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedTextComponent : MonoBehaviour
{
    [Header("Localized Text")]
    [TextArea(2, 4)]
    public string portugueseText;
    
    [TextArea(2, 4)]
    public string englishText;
    
    private TMP_Text textComponent;
    
    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        UpdateText();
        
        // Subscribe to language changes
        if (LanguageManager.Instance != null)
        {
            LanguageManager.OnLanguageChanged += UpdateText;
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from language changes
        LanguageManager.OnLanguageChanged -= UpdateText;
    }
    
    void UpdateText()
    {
        if (textComponent == null) return;
        
        if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
        {
            textComponent.text = englishText;
        }
        else
        {
            textComponent.text = portugueseText;
        }
    }
    
    public void SetText(string portuguese, string english)
    {
        portugueseText = portuguese;
        englishText = english;
        UpdateText();
    }
}