using UnityEngine;
using TMPro;
using RealGames;
using System.Collections;

public class ResultScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text profileNameText;
    public TMP_Text profileDescriptionText;
    public TMP_Text instructionText;
    public TMP_Text restartInstructionText;
    
    [Header("Settings")]
    [SerializeField] private float autoReturnTime = 10f; // Default fallback value
    
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
        SetupResultScreen();
        StartCoroutine(AutoReturnToIdle());
    }
    
    void RefreshTexts()
    {
        SetupResultScreen();
    }
    
    void SetupResultScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        ProfileInfo finalProfile = DilemmaGameController.Instance.GetFinalProfile();
        if (finalProfile == null) return;
        
        if (titleText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                titleText.text = "YOUR PROFILE";
            else
                titleText.text = "SEU PERFIL";
        }
            
        if (profileNameText != null)
            profileNameText.text = finalProfile.name.GetText();
            
        if (profileDescriptionText != null)
            profileDescriptionText.text = finalProfile.description.GetText();
            
        if (instructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                instructionText.text = "Thank you for participating!";
            else
                instructionText.text = "Obrigado por participar!";
        }
            
        if (restartInstructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                restartInstructionText.text = "Press 3 to play again";
            else
                restartInstructionText.text = "Pressione 3 para jogar novamente";
        }
    }
    
    IEnumerator AutoReturnToIdle()
    {
        // Get the result display time from the configuration
        if (DilemmaGameController.Instance != null)
        {
            autoReturnTime = DilemmaGameController.Instance.GetResultDisplayTime();
        }
        
        yield return new WaitForSeconds(autoReturnTime);
        
        if (DilemmaGameController.Instance != null)
        {
            DilemmaGameController.Instance.ResetToIdle();
        }
    }
}