using UnityEngine;
using TMPro;
using RealGames;
using System.Collections;
using UnityEngine.UI;

public class ResultScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text profileNameText;
    public TMP_Text profileDescriptionText;
    public TMP_Text instructionText;
    public TMP_Text restartInstructionText;
    public TMP_Text nfcInstructionText;
    public TMP_Text nfcFeedbackText;
    public TMP_Text scoreDisplayText;

    [Header("Score Fill Images")]
    public Image logicalReasoningFillImage;
    public Image selfAwarenessFillImage;
    public Image decisionMakingFillImage;

    [Header("Settings")]
    [SerializeField] private float autoReturnTime = 10f;
    [SerializeField] private float fillAnimationDuration = 0.35f;
    [SerializeField] private float delayBetweenFills = 0;

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
        ResetFillImages();
        SetupResultScreen();

        if (nfcFeedbackText != null)
        {
            nfcFeedbackText.text = "";
        }

        StartCoroutine(AnimateScoreFills());
        StartCoroutine(AutoReturnToIdle());
    }

    void ResetFillImages()
    {
        if (logicalReasoningFillImage != null)
            logicalReasoningFillImage.fillAmount = 0f;

        if (selfAwarenessFillImage != null)
            selfAwarenessFillImage.fillAmount = 0f;

        if (decisionMakingFillImage != null)
            decisionMakingFillImage.fillAmount = 0f;
    }

    IEnumerator AnimateScoreFills()
    {
        Debug.Log("[ResultScreen] Iniciando animação dos fills até 100%");

        yield return StartCoroutine(AnimateSingleFill(logicalReasoningFillImage, 1f));

        yield return new WaitForSeconds(delayBetweenFills);
        yield return StartCoroutine(AnimateSingleFill(selfAwarenessFillImage, 1f));

        yield return new WaitForSeconds(delayBetweenFills);
        yield return StartCoroutine(AnimateSingleFill(decisionMakingFillImage, 1f));
    }

    IEnumerator AnimateSingleFill(Image fillImage, float targetAmount)
    {
        if (fillImage == null)
        {
            Debug.LogWarning($"[ResultScreen] Fill image está null");
            yield break;
        }

        float elapsedTime = 0f;
        float startAmount = 0f;

        while (elapsedTime < fillAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fillAnimationDuration;
            fillImage.fillAmount = Mathf.Lerp(startAmount, targetAmount, progress);
            yield return null;
        }

        fillImage.fillAmount = targetAmount;
        Debug.Log($"[ResultScreen] Fill completo: {fillImage.name} = {targetAmount * 100}%");
    }

    void RefreshTexts()
    {
        SetupResultScreen();
        DisplayScoreInfo();
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

        if (nfcInstructionText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                nfcInstructionText.text = "Press 4 to activate NFC or use your NFC card to save your score!";
            else
                nfcInstructionText.text = "Pressione 4 para ativar NFC ou use seu cartão NFC para salvar sua pontuação!";
        }

        if (nfcFeedbackText != null)
        {
            nfcFeedbackText.text = "";
        }

        DisplayScoreInfo();
    }

    IEnumerator AutoReturnToIdle()
    {
        if (DilemmaGameController.Instance != null)
        {
            autoReturnTime = DilemmaGameController.Instance.GetResultDisplayTime();
        }

        yield return new WaitForSeconds(3f);

        if (NFCGameManager.Instance != null)
        {
            NFCGameManager.Instance.StartNFCSession();
        }

        yield return new WaitForSeconds(autoReturnTime - 3f);

        if (DilemmaGameController.Instance != null)
        {
            DilemmaGameController.Instance.ResetToIdle();
        }
    }

    public void ShowNFCWaitingFeedback()
    {
        if (nfcFeedbackText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                nfcFeedbackText.text = "Waiting for NFC card...";
            else
                nfcFeedbackText.text = "Aguardando cartão NFC...";
        }
    }

    public void ShowNFCSavedFeedback()
    {
        if (nfcFeedbackText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                nfcFeedbackText.text = "Data saved successfully!";
            else
                nfcFeedbackText.text = "Dados gravados com sucesso!";
        }
    }

    public void ShowNFCErrorFeedback()
    {
        if (nfcFeedbackText != null)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                nfcFeedbackText.text = "Error saving data. Try again.";
            else
                nfcFeedbackText.text = "Erro ao salvar dados. Tente novamente.";
        }
    }

    public void ClearNFCFeedback()
    {
        if (nfcFeedbackText != null)
        {
            nfcFeedbackText.text = "";
        }
    }

    void DisplayScoreInfo()
    {
        if (scoreDisplayText == null || DilemmaGameController.Instance == null || NFCGameManager.Instance == null)
            return;

        bool isRealist = DilemmaGameController.Instance.realistAnswers > DilemmaGameController.Instance.empatheticAnswers;

        NFCGameManager.Instance.GetScoreMapping(isRealist, out int logicalReasoning, out int selfAwareness, out int decisionMaking);

        string profileType;
        if (isRealist)
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                profileType = "Realist";
            else
                profileType = "Realista";
        }
        else
        {
            if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
                profileType = "Empathetic";
            else
                profileType = "Empático";
        }

        string scoreText;
        if (LanguageManager.Instance != null && LanguageManager.Instance.IsEnglish())
        {
            scoreText = $"<b>{profileType} Profile Scores:</b>\n" +
                       $"• Logical Reasoning: <color=#4CAF50>{logicalReasoning}</color>\n" +
                       $"• Self-Awareness: <color=#2196F3>{selfAwareness}</color>\n" +
                       $"• Decision Making: <color=#FF9800>{decisionMaking}</color>";
        }
        else
        {
            scoreText = $"<b>Pontuações do Perfil {profileType}:</b>\n" +
                       $"• Raciocínio Lógico: <color=#4CAF50>{logicalReasoning}</color>\n" +
                       $"• Autoconsciência: <color=#2196F3>{selfAwareness}</color>\n" +
                       $"• Tomada de Decisão: <color=#FF9800>{decisionMaking}</color>";
        }

        scoreDisplayText.text = scoreText;
    }
}
