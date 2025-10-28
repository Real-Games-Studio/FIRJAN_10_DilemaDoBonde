using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceScreen : CanvasScreen
{
    [Header("UI References")]
    public TMP_Text choiceConfirmationText;
    public TMP_Text waitingText;
    public TMP_Text skipInstructionText;
    public Image optionAFillImage;
    public Image optionBFillImage;

    private bool chosenOptionA = false;
    private float cooldownTime;
    private bool hasChosenOption = false;

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

        Debug.Log($"[ChoiceScreen] TurnOn chamado - hasChosenOption: {hasChosenOption}, chosenOptionA: {chosenOptionA}");

        ResetFillImages();
        SetupChoiceScreen();

        if (DilemmaGameController.Instance != null)
        {
            cooldownTime = DilemmaGameController.Instance.GetChoiceDisplayTime();
            Debug.Log($"[ChoiceScreen] TurnOn - cooldownTime: {cooldownTime}");
        }

        if (hasChosenOption)
        {
            StartFillAnimation();
        }
        else
        {
            Debug.LogWarning("[ChoiceScreen] TurnOn chamado mas SetChosenOption ainda não foi chamado!");
        }
    }

    void ResetFillImages()
    {
        if (optionAFillImage != null)
            optionAFillImage.fillAmount = 0f;

        if (optionBFillImage != null)
            optionBFillImage.fillAmount = 0f;
    }

    public void SetChosenOption(bool chooseOptionA)
    {
        chosenOptionA = chooseOptionA;
        hasChosenOption = true;
        Debug.Log($"[ChoiceScreen] SetChosenOption chamado - Opção escolhida: {(chooseOptionA ? "A" : "B")}, GameObject: {gameObject.name}");

        if (IsOn())
        {
            Debug.Log("[ChoiceScreen] Tela já está ativa, iniciando animação imediatamente");
            if (DilemmaGameController.Instance != null)
            {
                cooldownTime = DilemmaGameController.Instance.GetChoiceDisplayTime();
            }
            StartFillAnimation();
        }
    }

    public override void TurnOff()
    {
        base.TurnOff();
        hasChosenOption = false;
        StopAllCoroutines();
    }

    void StartFillAnimation()
    {
        Debug.Log($"[ChoiceScreen] StartFillAnimation - chosenOptionA: {chosenOptionA}");
        StopAllCoroutines();
        StartCoroutine(AnimateFillDuringCooldown());
    }

    private IEnumerator AnimateFillDuringCooldown()
    {
        Image targetFillImage = chosenOptionA ? optionAFillImage : optionBFillImage;

        Debug.Log($"[ChoiceScreen] AnimateFillDuringCooldown - Imagem alvo: {(targetFillImage != null ? targetFillImage.name : "NULL")}, cooldownTime: {cooldownTime}, opção: {(chosenOptionA ? "A" : "B")}");

        if (targetFillImage != null && cooldownTime > 0)
        {
            float elapsedTime = 0f;

            while (elapsedTime < cooldownTime)
            {
                elapsedTime += Time.deltaTime;
                targetFillImage.fillAmount = Mathf.Clamp01(elapsedTime / cooldownTime);
                yield return null;
            }

            targetFillImage.fillAmount = 1f;
            Debug.Log($"[ChoiceScreen] Animação completa para {targetFillImage.name}");
        }
        else
        {
            Debug.LogError($"[ChoiceScreen] Não pode animar - targetFillImage null: {targetFillImage == null}, cooldownTime: {cooldownTime}");
        }
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
