using UnityEngine;

[System.Serializable]
public class NFCServerConfig
{
    [Header("Server Settings")]
    public string serverIP = "127.0.0.1";
    public int serverPort = 8080;
    
    [Header("Game Configuration")]
    public int gameId = 10; // Dilema do bonde
    
    [Header("Score Mapping - Realista")]
    public int realistLogicalReasoning = 9;
    public int realistSelfAwareness = 5;
    public int realistDecisionMaking = 6;
    
    [Header("Score Mapping - Empático")]
    public int empatheticLogicalReasoning = 8;
    public int empatheticSelfAwareness = 6;
    public int empatheticDecisionMaking = 6;
    
    [Header("UI Configuration")]
    public float nfcTimeoutSeconds = 30f;
    public bool autoStartNFCAfterResults = true;
    public float nfcDelayAfterResults = 3f;
}

[CreateAssetMenu(fileName = "NFCServerConfig", menuName = "Dilemma Game/NFC Server Configuration")]
public class NFCServerConfigAsset : ScriptableObject
{
    public NFCServerConfig config;
    
    public static NFCServerConfigAsset LoadFromResources()
    {
        return Resources.Load<NFCServerConfigAsset>("NFCServerConfig");
    }
}