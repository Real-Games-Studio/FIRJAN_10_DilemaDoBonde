using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor utility for managing static texts
/// </summary>
public class StaticTextManagerEditor : EditorWindow
{
    [MenuItem("Tools/Static Text Manager")]
    public static void ShowWindow()
    {
        GetWindow<StaticTextManagerEditor>("Static Text Manager");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Static Text Manager", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Reload Static Texts"))
        {
            StaticTextManager.ReloadTexts();
            Debug.Log("Static texts reloaded from JSON file");
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Show All Text IDs"))
        {
            string[] textIds = StaticTextManager.GetAllTextIds();
            
            if (textIds.Length == 0)
            {
                Debug.Log("No static texts found");
                return;
            }
            
            Debug.Log("Available Text IDs:");
            for (int i = 0; i < textIds.Length; i++)
            {
                Debug.Log($"- {textIds[i]}");
            }
        }
        
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "JSON File Location: Assets/StreamingAssets/StaticTexts.json\n\n" +
            "To add new texts:\n" +
            "1. Edit the JSON file\n" +
            "2. Add new entries with id, textPt, and textEn\n" +
            "3. Click 'Reload Static Texts'\n\n" +
            "To use in UI:\n" +
            "1. Add UITextLocalizer component to Text/TextMeshPro\n" +
            "2. Set the Text ID in the component\n" +
            "3. Text will update automatically on language change",
            MessageType.Info
        );
    }
}