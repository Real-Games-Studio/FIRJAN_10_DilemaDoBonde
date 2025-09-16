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
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupDilemmaScreen();
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
    }
}