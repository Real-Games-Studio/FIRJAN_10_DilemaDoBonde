using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class ScreenSystemValidator : MonoBehaviour
{
    [MenuItem("Tools/Validate Screen System")]
    public static void ValidateScreenSystem()
    {
        Debug.Log("====================================");
        Debug.Log("[ScreenSystemValidator] Starting validation...");
        Debug.Log("====================================");
        
        ScreenCanvasController controller = Object.FindAnyObjectByType<ScreenCanvasController>();
        
        if (controller == null)
        {
            Debug.LogError("[ScreenSystemValidator] FAILED - ScreenCanvasController not found in scene!");
            EditorUtility.DisplayDialog("Validation Failed", 
                "ScreenCanvasController not found in the scene!", 
                "OK");
            return;
        }
        
        string initialScreen = controller.inicialScreen;
        Debug.Log($"[ScreenSystemValidator] Controller initial screen: '{initialScreen}'");
        
        if (string.IsNullOrEmpty(initialScreen))
        {
            Debug.LogError("[ScreenSystemValidator] FAILED - Initial screen is NULL or EMPTY!");
            EditorUtility.DisplayDialog("Validation Failed", 
                "Initial screen in ScreenCanvasController is empty!", 
                "OK");
            EditorGUIUtility.PingObject(controller);
            return;
        }
        
        CanvasScreen[] allScreens = Object.FindObjectsByType<CanvasScreen>(FindObjectsSortMode.None);
        Debug.Log($"[ScreenSystemValidator] Found {allScreens.Length} CanvasScreen components");
        
        bool foundInitialScreen = false;
        int validScreens = 0;
        int invalidScreens = 0;
        
        foreach (var screen in allScreens)
        {
            string screenName = screen.screenData?.screenName;
            
            if (string.IsNullOrEmpty(screenName))
            {
                Debug.LogError($"[ScreenSystemValidator] FAILED - Screen '{screen.gameObject.name}' has NULL or EMPTY screenName!", screen);
                invalidScreens++;
                continue;
            }
            
            Debug.Log($"[ScreenSystemValidator] Valid screen: '{screenName}' on GameObject '{screen.gameObject.name}'");
            validScreens++;
            
            if (screenName == initialScreen)
            {
                foundInitialScreen = true;
                Debug.Log($"[ScreenSystemValidator] ✓ Initial screen '{initialScreen}' found!");
            }
        }
        
        Debug.Log("====================================");
        Debug.Log($"[ScreenSystemValidator] Valid screens: {validScreens}");
        Debug.Log($"[ScreenSystemValidator] Invalid screens: {invalidScreens}");
        Debug.Log("====================================");
        
        if (!foundInitialScreen)
        {
            Debug.LogError($"[ScreenSystemValidator] CRITICAL ERROR - Initial screen '{initialScreen}' NOT FOUND in scene!");
            Debug.LogError($"[ScreenSystemValidator] Available screens:");
            
            foreach (var screen in allScreens)
            {
                string screenName = screen.screenData?.screenName;
                if (!string.IsNullOrEmpty(screenName))
                {
                    Debug.LogError($"  - '{screenName}'");
                }
            }
            
            EditorUtility.DisplayDialog("Validation Failed", 
                $"Initial screen '{initialScreen}' does not exist!\n\n" +
                $"Please set the initial screen to one of the available screens listed in the Console.", 
                "OK");
            EditorGUIUtility.PingObject(controller);
            return;
        }
        
        if (invalidScreens > 0)
        {
            Debug.LogWarning($"[ScreenSystemValidator] WARNING - {invalidScreens} screen(s) have invalid configuration!");
            EditorUtility.DisplayDialog("Validation Warning", 
                $"{invalidScreens} screen(s) have empty screenName fields.\n\n" +
                "Check the Console for details.", 
                "OK");
            return;
        }
        
        Debug.Log("[ScreenSystemValidator] ✓✓✓ ALL VALIDATIONS PASSED! ✓✓✓");
        EditorUtility.DisplayDialog("Validation Successful", 
            "Screen system is properly configured!\n\n" +
            $"Initial screen: {initialScreen}\n" +
            $"Total screens: {validScreens}", 
            "OK");
    }
    
    [MenuItem("Tools/Fix Screen System - Set Initial to IdleScreen")]
    public static void FixToIdleScreen()
    {
        ScreenCanvasController controller = Object.FindAnyObjectByType<ScreenCanvasController>();
        
        if (controller == null)
        {
            Debug.LogError("[ScreenSystemValidator] ScreenCanvasController not found!");
            return;
        }
        
        Undo.RecordObject(controller, "Fix Initial Screen");
        controller.inicialScreen = "IdleScreen";
        EditorUtility.SetDirty(controller);
        
        Debug.Log("[ScreenSystemValidator] Initial screen set to 'IdleScreen'");
        EditorUtility.DisplayDialog("Fixed", 
            "Initial screen has been set to 'IdleScreen'", 
            "OK");
    }
}
#endif
