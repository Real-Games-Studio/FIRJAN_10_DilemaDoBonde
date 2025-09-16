using UnityEngine;
using TMPro;

public class IdleScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text instructionText;
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupIdleScreen();
    }
    
    void SetupIdleScreen()
    {
        if (titleText != null)
            titleText.text = "DILEMA DO BONDE";
            
        if (instructionText != null)
            instructionText.text = "Pressione 1 ou 2 para começar";
    }
}