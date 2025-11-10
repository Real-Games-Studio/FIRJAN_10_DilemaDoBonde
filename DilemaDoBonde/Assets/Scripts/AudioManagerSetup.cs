using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class AudioManagerSetup : MonoBehaviour
{
    [MenuItem("GameObject/Audio/Setup AudioManager", false, 10)]
    static void CreateAudioManager(MenuCommand menuCommand)
    {
        GameObject existingManager = GameObject.Find("AudioManager");
        if (existingManager != null)
        {
            Debug.LogWarning("AudioManager already exists in the scene!");
            Selection.activeGameObject = existingManager;
            return;
        }
        
        GameObject audioManagerObj = new GameObject("AudioManager");
        AudioManager audioManager = audioManagerObj.AddComponent<AudioManager>();
        
        GameObjectUtility.SetParentAndAlign(audioManagerObj, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(audioManagerObj, "Create AudioManager");
        Selection.activeObject = audioManagerObj;
        
        Debug.Log("AudioManager created successfully! Add your audio clips in the Inspector or place them in Resources/Audio/SFX/");
    }
}
#endif
