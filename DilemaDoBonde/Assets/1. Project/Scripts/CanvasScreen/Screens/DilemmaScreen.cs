using UnityEngine;
using TMPro;
using RealGames;

public class DilemmaScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text questionText;
    public TMP_Text optionAText;
    public TMP_Text optionBText;
    public TMP_Text instructionText;
    public TMP_Text timeRemainingText;
    public TMP_Text progressText;
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupDilemmaScreen();
    }
    
    void Update()
    {
        UpdateTimerDisplay();
    }
    
    void SetupDilemmaScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        DilemmaData currentDilemma = DilemmaGameController.Instance.GetCurrentDilemma();
        if (currentDilemma == null) return;
        
        if (titleText != null)
            titleText.text = currentDilemma.title;
            
        if (descriptionText != null)
            descriptionText.text = currentDilemma.description;
            
        if (questionText != null)
            questionText.text = currentDilemma.question;
            
        if (optionAText != null)
            optionAText.text = currentDilemma.optionA.text;
            
        if (optionBText != null)
            optionBText.text = currentDilemma.optionB.text;
            
        if (instructionText != null)
            instructionText.text = "Pressione 1 para A ou 2 para B";
            
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
                timeRemainingText.text = $"{seconds}s";
            }
            else
            {
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