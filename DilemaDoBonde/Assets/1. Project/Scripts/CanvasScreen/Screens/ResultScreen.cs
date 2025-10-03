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
    
    [Header("Settings")]
    [SerializeField] private float autoReturnTime = 10f; // Default fallback value
    
    public override void TurnOn()
    {
        base.TurnOn();
        SetupResultScreen();
        StartCoroutine(AutoReturnToIdle());
    }
    
    void SetupResultScreen()
    {
        if (DilemmaGameController.Instance == null) return;
        
        ProfileInfo finalProfile = DilemmaGameController.Instance.GetFinalProfile();
        if (finalProfile == null) return;
        
        if (titleText != null)
            titleText.text = "SEU PERFIL";
            
        if (profileNameText != null)
            profileNameText.text = finalProfile.name;
            
        if (profileDescriptionText != null)
            profileDescriptionText.text = finalProfile.description;
            
        if (instructionText != null)
            instructionText.text = "Obrigado por participar!";
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