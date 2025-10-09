using UnityEngine;

public class DilemmaInputHandler : MonoBehaviour
{
    private float lastHorizontalInput = 0f;
    private bool horizontalInputProcessed = false;
    
    void Update()
    {
        HandleKeyboardInput();
        HandleHorizontalAxisInput();
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
        
        // Mantém suporte para Fire3 e Fire1 para tecla 3
        if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            OnNumberPressed(3);
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
}