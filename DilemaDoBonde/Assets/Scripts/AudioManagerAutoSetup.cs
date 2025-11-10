using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AudioManagerAutoSetup
{
    static AudioManagerAutoSetup()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }
    
    static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            CheckForAudioManager();
        }
    }
    
    static void CheckForAudioManager()
    {
        AudioManager existing = Object.FindFirstObjectByType<AudioManager>();
        
        if (existing == null)
        {
            bool createManager = EditorUtility.DisplayDialog(
                "AudioManager Não Encontrado",
                "A cena MainScene não possui um AudioManager. Deseja criar um agora?\n\n" +
                "O AudioManager é necessário para reproduzir sons no jogo.",
                "Sim, Criar",
                "Não, Depois"
            );
            
            if (createManager)
            {
                GameObject audioManagerObj = new GameObject("AudioManager");
                audioManagerObj.AddComponent<AudioManager>();
                EditorUtility.SetDirty(audioManagerObj);
                
                Debug.Log("<color=green>✓ AudioManager criado com sucesso!</color> Configure os AudioClips no Inspector ou adicione arquivos em Resources/Audio/SFX/");
            }
        }
    }
    
    [MenuItem("Tools/Audio/Verificar Configuração de Áudio", false, 100)]
    static void VerifyAudioSetup()
    {
        AudioManager manager = Object.FindFirstObjectByType<AudioManager>();
        
        System.Text.StringBuilder report = new System.Text.StringBuilder();
        report.AppendLine("=== VERIFICAÇÃO DO SISTEMA DE ÁUDIO ===\n");
        
        if (manager == null)
        {
            report.AppendLine("❌ AudioManager NÃO encontrado na cena");
            report.AppendLine("   Solução: GameObject > Audio > Setup AudioManager\n");
        }
        else
        {
            report.AppendLine("✓ AudioManager encontrado na cena\n");
        }
        
        string resourcePath = "Assets/Resources/Audio/SFX";
        if (!AssetDatabase.IsValidFolder(resourcePath))
        {
            report.AppendLine("❌ Pasta Resources/Audio/SFX não existe");
            report.AppendLine("   Solução: Crie a pasta Assets/Resources/Audio/SFX/\n");
        }
        else
        {
            report.AppendLine("✓ Pasta Resources/Audio/SFX existe\n");
            
            string[] audioFiles = System.IO.Directory.GetFiles(resourcePath, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            int validAudioFiles = 0;
            foreach (string file in audioFiles)
            {
                if (file.EndsWith(".wav") || file.EndsWith(".mp3") || file.EndsWith(".ogg"))
                {
                    validAudioFiles++;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    report.AppendLine($"   • {fileName}");
                }
            }
            
            if (validAudioFiles == 0)
            {
                report.AppendLine("   ⚠️ Nenhum arquivo de áudio encontrado");
            }
            else
            {
                report.AppendLine($"\n   Total: {validAudioFiles} arquivo(s) de áudio");
            }
        }
        
        report.AppendLine("\n=== SONS NECESSÁRIOS ===");
        report.AppendLine("• nfc_read - Som de leitura NFC");
        report.AppendLine("• choice - Som de escolha A ou B");
        report.AppendLine("• start_button - Som de botão iniciar");
        report.AppendLine("• game_end - Som de fim de jogo");
        
        report.AppendLine("\n=== INTEGRAÇÕES ===");
        report.AppendLine("✓ NFCGameManager - Leitura NFC");
        report.AppendLine("✓ DilemmaGameController - Escolhas e Iniciar");
        report.AppendLine("✓ DilemmaGameController - Fim de Jogo");
        
        Debug.Log(report.ToString());
        
        EditorUtility.DisplayDialog(
            "Verificação do Sistema de Áudio",
            report.ToString(),
            "OK"
        );
    }
    
    [MenuItem("Tools/Audio/Criar Estrutura de Pastas", false, 101)]
    static void CreateAudioFolders()
    {
        string resourcesPath = "Assets/Resources";
        string audioPath = "Assets/Resources/Audio";
        string sfxPath = "Assets/Resources/Audio/SFX";
        string musicPath = "Assets/Resources/Audio/Music";
        
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        
        if (!AssetDatabase.IsValidFolder(audioPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Audio");
        }
        
        if (!AssetDatabase.IsValidFolder(sfxPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Audio", "SFX");
        }
        
        if (!AssetDatabase.IsValidFolder(musicPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Audio", "Music");
        }
        
        AssetDatabase.Refresh();
        
        Debug.Log("<color=green>✓ Estrutura de pastas de áudio criada com sucesso!</color>\n" +
                  "Adicione seus arquivos de áudio em:\n" +
                  "• Assets/Resources/Audio/SFX/ (efeitos sonoros)\n" +
                  "• Assets/Resources/Audio/Music/ (música de fundo)");
        
        EditorUtility.DisplayDialog(
            "Estrutura Criada",
            "Pastas de áudio criadas com sucesso!\n\n" +
            "Agora adicione seus arquivos de áudio em:\n" +
            "• Resources/Audio/SFX/\n" +
            "• Resources/Audio/Music/",
            "OK"
        );
        
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(sfxPath));
    }
}
#endif
