using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class HoldToResetButton : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Tempo em segundos que o botão precisa ser segurado")]
    public float holdDuration = 3f;

    [Tooltip("Tecla do teclado para segurar (ex: Key.R)")]
    public Key resetKey = Key.R;

    [Header("Visual Feedback")]
    [Tooltip("Slider que mostra o progresso do hold")]
    public Slider progressSlider;

    [Tooltip("Imagem que mostra o progresso do hold")]
    public Image progressFillImage;

    [Tooltip("Texto que mostra o tempo restante")]
    public TMP_Text timerText;

    [Tooltip("Mensagem de instrução (opcional)")]
    public GameObject instructionMessage;

    private float currentHoldTime = 0f;
    private bool isHolding = false;

    void Start()
    {
        ResetVisuals();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[resetKey].isPressed)
        {
            if (!isHolding)
            {
                StartHolding();
            }

            UpdateHolding();
        }
        else
        {
            if (isHolding)
            {
                CancelHolding();
            }
        }
    }

    void StartHolding()
    {
        isHolding = true;
        currentHoldTime = 0f;

        if (instructionMessage != null)
            instructionMessage.SetActive(false);

        Debug.Log($"<color=yellow>[Hold To Reset]</color> Começou a segurar o botão...");
    }

    void UpdateHolding()
    {
        currentHoldTime += Time.deltaTime;

        float progress = Mathf.Clamp01(currentHoldTime / holdDuration);
        UpdateVisuals(progress);

        if (currentHoldTime >= holdDuration)
        {
            ExecuteReset();
        }
    }

    void CancelHolding()
    {
        Debug.Log($"<color=yellow>[Hold To Reset]</color> Botão solto - resetando progresso");
        isHolding = false;
        ResetVisuals();
    }

    void ExecuteReset()
    {
        Debug.Log($"<color=green>[Hold To Reset]</color> Botão segurado por {holdDuration}s - REINICIANDO JOGO!");
        
        isHolding = false;
        ResetVisuals();

        if (DilemmaGameController.Instance != null)
        {
            DilemmaGameController.Instance.ResetToIdle();
        }
        else
        {
            Debug.LogWarning("[Hold To Reset] DilemmaGameController.Instance não encontrado!");
        }
    }

    void UpdateVisuals(float progress)
    {
        if (progressSlider != null)
        {
            progressSlider.value = progress;
        }

        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = progress;
        }

        if (timerText != null)
        {
            float timeRemaining = holdDuration - currentHoldTime;
            timerText.text = $"{Mathf.CeilToInt(timeRemaining)}s";
        }
    }

    void ResetVisuals()
    {
        currentHoldTime = 0f;

        if (progressSlider != null)
        {
            progressSlider.value = 0f;
        }

        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = 0f;
        }

        if (timerText != null)
        {
            timerText.text = "";
        }

        if (instructionMessage != null)
            instructionMessage.SetActive(true);
    }
}
