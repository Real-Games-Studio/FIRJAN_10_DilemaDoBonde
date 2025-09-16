using UnityEngine;

public class DilemmaInputHandler : MonoBehaviour
{
    void Update()
    {
        HandleKeyboardInput();
    }
    
    void HandleKeyboardInput()
    {
        // Handle number keys 1 and 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNumberPressed(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnNumberPressed(2);
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
    }
    
    public void OnNumberPressed(int number)
    {
        Debug.Log($"Number {number} pressed");
        
        // Reset inactive timer in ScreenCanvasController
        if (ScreenCanvasController.instance != null)
        {
            ScreenCanvasController.instance.inactiveTimer = 0;
        }
        
        // Let DilemmaGameController handle the input logic
        if (DilemmaGameController.Instance != null)
        {
            // The DilemmaGameController already handles this in its Update method
            // This is just to ensure the input is properly detected
        }
    }
}