using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DilemmaInputHandler : MonoBehaviour
{
    [Header("Hold to Reset Settings")]
    [Tooltip("Tempo em segundos que o botão precisa ser segurado para resetar")]
    public float holdToResetDuration = 3f;
    
    [Header("Visual Feedback (Optional)")]
    [Tooltip("Slider que mostra o progresso do hold")]
    public Slider resetProgressSlider;
    
    [Tooltip("Imagem que mostra o progresso do hold")]
    public Image resetProgressFillImage;
    
    [Tooltip("Texto que mostra o tempo restante")]
    public TMP_Text resetTimerText;
    
    [Tooltip("Mensagem de instrução para reset")]
    public GameObject resetInstructionMessage;
    
    private float lastHorizontalInput = 0f;
    private bool horizontalInputProcessed = false;
    
    private float currentHoldTime = 0f;
    private bool isHoldingToReset = false;
    private bool resetExecuted = false;
    
    void Update()
    {
        HandleKeyboardInput();
        HandleHorizontalAxisInput();
        HandleHoldToReset();
    }
    
    void HandleHorizontalAxisInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Detectar mudança de estado - só processa quando há movimento novo
        if (!horizontalInputProcessed)
        {
            if (horizontalInput < -0.5f) // Esquerda
            {
                OnHorizontalInput(-1);
                horizontalInputProcessed = true;
            }
            else if (horizontalInput > 0.5f) // Direita
            {
                OnHorizontalInput(1);
                horizontalInputProcessed = true;
            }
        }
        
        // Reset quando voltar ao centro
        if (Mathf.Abs(horizontalInput) < 0.3f)
        {
            horizontalInputProcessed = false;
        }
        
        lastHorizontalInput = horizontalInput;
    }
    
    void OnHorizontalInput(int direction)
    {
        // Reset inactive timer in ScreenCanvasController
        if (ScreenCanvasController.instance != null)
        {
            ScreenCanvasController.instance.inactiveTimer = 0;
        }
        
        // Verificar se estamos na tela inicial para mudança de idioma
        if (DilemmaGameController.Instance != null)
        {
            string currentScreenName = ScreenManager.GetCurrentScreenName();
            
            if (currentScreenName == DilemmaGameController.Instance.idleScreenName)
            {
                // Na tela inicial: -1 = Português, 1 = Inglês
                if (direction == -1 && LanguageManager.Instance != null)
                {
                    LanguageManager.Instance.SetPortuguese();
                    Debug.Log("Idioma alterado para Português via joystick");
                }
                else if (direction == 1 && LanguageManager.Instance != null)
                {
                    LanguageManager.Instance.SetEnglish();
                    Debug.Log("Idioma alterado para Inglês via joystick");
                }
            }
            else
            {
                // Em outras telas: mapear para os números 1 e 2
                int mappedNumber = (direction == -1) ? 1 : 2;
                DilemmaGameController.Instance.OnNumberInput(mappedNumber);
            }
        }
    }
    
    void HandleKeyboardInput()
    {
        // Handle number keys 1, 2, and 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNumberPressed(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnNumberPressed(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnNumberPressed(3);
        }
        
        // Handle key 4 for NFC activation on result screen
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnNumberPressed(4);
        }
        
        // Handle key 5 for reloading server configuration (development only)
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ReloadServerConfig();
        }
        
        // Also handle keypad numbers
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            OnNumberPressed(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            OnNumberPressed(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            OnNumberPressed(3);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            OnNumberPressed(4);
        }
        
        // Mantém suporte para Fire3, Fire1 e Fire2 para tecla 3
        // Mas só dispara se for um clique rápido (não um hold)
        if (Input.GetButtonUp("Fire3") || Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            if (!resetExecuted && currentHoldTime < 0.5f)
            {
                OnNumberPressed(3);
                resetExecuted = false;
            }
        }
    }
    
    public void OnNumberPressed(int number)
    {
        // Reset inactive timer in ScreenCanvasController
        if (ScreenCanvasController.instance != null)
        {
            ScreenCanvasController.instance.inactiveTimer = 0;
        }
        
        // Delegate input handling to DilemmaGameController
        if (DilemmaGameController.Instance != null)
        {
            DilemmaGameController.Instance.OnNumberInput(number);
        }
    }
    
    void ReloadServerConfig()
    {
        if (NFCGameManager.Instance != null)
        {
            NFCGameManager.Instance.ReloadServerConfiguration();
        }
        else
        {
            Debug.LogWarning("NFCGameManager não encontrado para recarregar configuração");
        }
    }
    
    void HandleHoldToReset()
    {
        bool isButtonPressed = Input.GetButton("Fire3") || Input.GetButton("Fire1") || Input.GetButton("Fire2");
        
        if (isButtonPressed)
        {
            if (!isHoldingToReset && !resetExecuted)
            {
                StartHoldingToReset();
            }
            
            if (isHoldingToReset && !resetExecuted)
            {
                UpdateHoldToReset();
            }
        }
        else
        {
            if (isHoldingToReset && !resetExecuted)
            {
                CancelHoldToReset();
            }
        }
    }
    
    void StartHoldingToReset()
    {
        isHoldingToReset = true;
        currentHoldTime = 0f;
        resetExecuted = false;
        
        if (resetInstructionMessage != null)
            resetInstructionMessage.SetActive(false);
        
        Debug.Log("<color=yellow>[Hold To Reset]</color> Começou a segurar o botão para resetar...");
    }
    
    void UpdateHoldToReset()
    {
        currentHoldTime += Time.deltaTime;
        
        float progress = Mathf.Clamp01(currentHoldTime / holdToResetDuration);
        UpdateResetVisuals(progress);
        
        if (currentHoldTime >= holdToResetDuration && !resetExecuted)
        {
            ExecuteReset();
        }
    }
    
    void CancelHoldToReset()
    {
        Debug.Log("<color=yellow>[Hold To Reset]</color> Botão solto antes de completar - resetando progresso");
        isHoldingToReset = false;
        resetExecuted = false;
        ResetVisuals();
    }
    
    void ExecuteReset()
    {
        Debug.Log("<color=green>[Hold To Reset]</color> Botão segurado por " + holdToResetDuration + "s - REINICIANDO JOGO!");
        
        resetExecuted = true;
        isHoldingToReset = false;
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
    
    void UpdateResetVisuals(float progress)
    {
        if (resetProgressSlider != null)
        {
            resetProgressSlider.value = progress;
        }
        
        if (resetProgressFillImage != null)
        {
            resetProgressFillImage.fillAmount = progress;
        }
        
        if (resetTimerText != null)
        {
            float timeRemaining = holdToResetDuration - currentHoldTime;
            resetTimerText.text = Mathf.CeilToInt(timeRemaining) + "s";
        }
    }
    
    void ResetVisuals()
    {
        currentHoldTime = 0f;
        
        if (resetProgressSlider != null)
        {
            resetProgressSlider.value = 0f;
        }
        
        if (resetProgressFillImage != null)
        {
            resetProgressFillImage.fillAmount = 0f;
        }
        
        if (resetTimerText != null)
        {
            resetTimerText.text = "";
        }
        
        if (resetInstructionMessage != null)
            resetInstructionMessage.SetActive(true);
    }
}