using UnityEngine;
using TMPro;

public class ChoiceScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text choiceConfirmationText;
    public TMP_Text waitingText;
    public TMP_Text skipInstructionText;
    
    public override void OnEnable()
    {
        base.OnEnable();
        LanguageManager.OnLanguageChanged += RefreshTexts;
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        LanguageManager.OnLanguageChanged -= RefreshTexts;
    }
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupChoiceScreen();
    }
    
    void RefreshTexts()
    {
        SetupChoiceScreen();
    }
    
    void SetupChoiceScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        string lastChoice = DilemmaGameController.Instance.GetLastChoice();
        
        if (choiceConfirmationText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                choiceConfirmationText.text = "Choice recorded!";
            else
                choiceConfirmationText.text = "Programação gravada!";
        }
            
        if (waitingText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                waitingText.text = "Preparing next dilemma...";
            else
                waitingText.text = "Preparando próximo dilema...";
        }
            
        if (skipInstructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                skipInstructionText.text = "Press 3 to skip to next dilemma";
            else
                skipInstructionText.text = "Pressione 3 para pular para o próximo dilema";
        }
    }
}