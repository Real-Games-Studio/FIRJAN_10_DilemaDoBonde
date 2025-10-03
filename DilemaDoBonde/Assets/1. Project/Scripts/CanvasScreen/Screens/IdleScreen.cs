using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IdleScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text instructionText;
    
    [Header("Language Buttons")]
    public Button portugueseButton;
    public Button englishButton;
    public TMP_Text portugueseButtonText;
    public TMP_Text englishButtonText;
    
    public override void OnEnable()
    {
        base.OnEnable();
        LanguageManager.OnLanguageChanged += RefreshTexts;
        SetupLanguageButtons();
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        LanguageManager.OnLanguageChanged -= RefreshTexts;
    }
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupIdleScreen();
    }
    
    void SetupLanguageButtons()
    {
        if (portugueseButton != null)
            portugueseButton.onClick.AddListener(() => LanguageManager.Instance?.SetPortuguese());
        portugueseButton.onClick.AddListener(() => Debug.Log("asdasd"));


        if (englishButton != null)
            englishButton.onClick.AddListener(() => LanguageManager.Instance?.SetEnglish());
    }
    
    void RefreshTexts()
    {
        SetupIdleScreen();
    }
    
    void SetupIdleScreen()
    {
        if (titleText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                titleText.text = "TROLLEY PROBLEM";
            else
                titleText.text = "DILEMA DO BONDE";
        }
            
        if (instructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                instructionText.text = "Press button to start";
            else
                instructionText.text = "Pressione o botão para começar";
        }
        
        // Update language button texts
        if (portugueseButtonText != null)
            portugueseButtonText.text = "PT";
            
        if (englishButtonText != null)
            englishButtonText.text = "EN";
    }
}