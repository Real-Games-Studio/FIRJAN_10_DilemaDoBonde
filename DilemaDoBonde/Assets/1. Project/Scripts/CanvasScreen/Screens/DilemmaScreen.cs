using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RealGames;

public class DilemmaScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text questionText;
    public TMP_Text leftOptionText;   // New: text for left position
    public TMP_Text rightOptionText;  // New: text for right position
    public TMP_Text leftOptionLabel;  // New: displays A or B for left option
    public TMP_Text rightOptionLabel; // New: displays A or B for right option
    
    [Header("Legacy UI References (deprecated)")]
    public TMP_Text optionAText;      // Legacy: kept for backward compatibility
    public TMP_Text optionBText;      // Legacy: kept for backward compatibility
    
    [Header("Other UI References")]
    public TMP_Text instructionText;
    public TMP_Text timeRemainingText;
    public TMP_Text progressText;
    public Image timerFillImage;
    
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
        SetupDilemmaScreen();
    }
    
    void Update()
    {
        UpdateTimerDisplay();
        UpdateTimerFill();
    }
    
    void UpdateTimerFill()
    {
        if (timerFillImage != null && DilemmaGameController.Instance != null)
        {
            float remainingTime = DilemmaGameController.Instance.GetRemainingTime();
            float totalTime = DilemmaGameController.Instance.dilemmaConfig.timeoutSeconds;
            
            if (totalTime > 0)
            {
                timerFillImage.fillAmount = Mathf.Clamp01(remainingTime / totalTime);
            }
        }
    }
    
    void RefreshTexts()
    {
        SetupDilemmaScreen();
    }
    
    void SetupDilemmaScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        DilemmaData currentDilemma = DilemmaGameController.Instance.GetCurrentDilemma();
        if (currentDilemma == null) return;
        
        if (titleText != null)
            titleText.text = currentDilemma.title.GetText();
            
        if (descriptionText != null)
            descriptionText.text = currentDilemma.description.GetText();
            
        if (questionText != null)
            questionText.text = currentDilemma.question.GetText();
        
        // Use randomized positions for options
        DilemmaOption leftOption = DilemmaGameController.Instance.GetLeftOption();
        DilemmaOption rightOption = DilemmaGameController.Instance.GetRightOption();
        
        // Update new position-based text fields
        if (leftOptionText != null && leftOption != null)
            leftOptionText.text = leftOption.text.GetText();
            
        if (rightOptionText != null && rightOption != null)
            rightOptionText.text = rightOption.text.GetText();
        
        // Update option labels (A or B)
        if (leftOptionLabel != null)
            leftOptionLabel.text = DilemmaGameController.Instance.GetLeftOptionLabel();
            
        if (rightOptionLabel != null)
            rightOptionLabel.text = DilemmaGameController.Instance.GetRightOptionLabel();
        
        // Legacy support: Update old optionAText and optionBText if they exist
        if (optionAText != null)
            optionAText.text = currentDilemma.optionA.text.GetText();
            
        if (optionBText != null)
            optionBText.text = currentDilemma.optionB.text.GetText();
            
        if (instructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                instructionText.text = "Press 1 for left or 2 for right";
            else
                instructionText.text = "Pressione 1 para esquerda ou 2 para direita";
        }
            
        UpdateProgressDisplay();
    }
    
    void UpdateTimerDisplay()
    {
        if (timeRemainingText != null && DilemmaGameController.Instance != null)
        {
            float remainingTime = DilemmaGameController.Instance.GetRemainingTime();
            if (remainingTime > 0)
            {
                int seconds = Mathf.CeilToInt(remainingTime);
                
                string timeText;
                if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                    timeText = $"{seconds}s";
                else
                    timeText = $"{seconds}s";
                    
                timeRemainingText.text = timeText;
            }
            else
            {
                if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                    timeRemainingText.text = "Time's up!";
                else
                    timeRemainingText.text = "Tempo esgotado!";
            }
        }
    }
    
    void UpdateProgressDisplay()
    {
        if (progressText != null && DilemmaGameController.Instance != null)
        {
            int currentIndex = DilemmaGameController.Instance.currentDilemmaIndex + 1;
            int totalDilemmas = DilemmaGameController.Instance.GetTotalDilemmas();
            progressText.text = $"{currentIndex}/{totalDilemmas}";
        }
    }
}