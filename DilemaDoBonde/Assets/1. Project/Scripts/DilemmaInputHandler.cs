using UnityEngine;

public class DilemmaInputHandler : MonoBehaviour
{
    void Update()
    {
        HandleKeyboardInput();
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
        
        // Handle Fire inputs (Fire1 = Left Ctrl/Mouse0, Fire2 = Left Alt/Mouse1, Fire3 = Left Shift/Mouse2)
        if (Input.GetButtonDown("Fire1"))
        {
            OnNumberPressed(1);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            OnNumberPressed(2);
        }
        else if (Input.GetButtonDown("Fire3"))
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
}