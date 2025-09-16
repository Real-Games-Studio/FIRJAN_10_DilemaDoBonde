using UnityEngine;
using TMPro;

public class ChoiceScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text choiceConfirmationText;
    public TMP_Text waitingText;
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupChoiceScreen();
    }
    
    void SetupChoiceScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        string lastChoice = DilemmaGameController.Instance.GetLastChoice();
        
        if (choiceConfirmationText != null)
            choiceConfirmationText.text = "Programação gravada!";
            
        if (waitingText != null)
            waitingText.text = "Preparando próximo dilema...";
    }
}