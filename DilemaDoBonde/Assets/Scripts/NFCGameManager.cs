using System.Collections;
using UnityEngine;
using _4._NFC_Firjan.Scripts.NFC;
using _4._NFC_Firjan.Scripts.Server;
using System.Net;
using TMPro;
using UnityEngine.UI;
using RealGames;

public class NFCGameManager : MonoBehaviour
{
    [Header("NFC Configuration")]
    public NFCReceiver nfcReceiver;
    public ServerComunication serverCommunication;
    
    [Header("Server Configuration")]
    [Tooltip("IP e porta serão carregados do config.json automaticamente")]
    public string serverIP = "192.168.0.185";
    public int serverPort = 8080;
    
    [Header("Game Configuration")]
    public int gameId = 10; // Dilema do bonde conforme documentação
    
    [Header("UI References")]
    public GameObject nfcPromptPanel;
    public TMP_Text nfcStatusText;
    public TMP_Text nfcInstructionText;
    public Button skipNFCButton;
    
    [Header("Score Mapping - Realista")]
    public int realistLogicalReasoning = 9;
    public int realistSelfAwareness = 5;
    public int realistDecisionMaking = 6;
    
    [Header("Score Mapping - Empático")]
    public int empatheticLogicalReasoning = 8;
    public int empatheticSelfAwareness = 6;
    public int empatheticDecisionMaking = 6;
    
    private string currentNFCId;
    private string currentNFCReader;
    private bool isWaitingForNFC = false;
    private bool gameResultsSent = false;
    
    public static NFCGameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializeNFC();
        InitializeServer();
        SetupUI();
    }
    
    void InitializeNFC()
    {
        if (nfcReceiver == null)
        {
            // Tentar encontrar automaticamente o NFCReceiver na cena
            nfcReceiver = Object.FindFirstObjectByType<NFCReceiver>();
        }
        
        if (nfcReceiver != null)
        {
            nfcReceiver.OnNFCConnected.AddListener(OnNFCConnected);
            nfcReceiver.OnNFCDisconnected.AddListener(OnNFCDisconnected);
            nfcReceiver.OnNFCReaderConnected.AddListener(OnNFCReaderConnected);
            nfcReceiver.OnNFCReaderDisconected.AddListener(OnNFCReaderDisconnected);
            Debug.Log("NFCReceiver inicializado com sucesso!");
        }
        else
        {
            Debug.LogError("NFCReceiver não encontrado! Certifique-se de que há um GameObject com NFCReceiver na cena.");
        }
    }
    
    void InitializeServer()
    {
        if (serverCommunication == null)
        {
            // Tentar encontrar automaticamente o ServerComunication na cena
            serverCommunication = Object.FindFirstObjectByType<ServerComunication>();
        }
        
        if (serverCommunication != null)
        {
            // Carregar configuração do JSON
            var serverConfig = ServerConfig.Server;
            serverIP = serverConfig.ip;
            serverPort = serverConfig.port;
            
            serverCommunication.Ip = serverIP;
            serverCommunication.Port = serverPort;
            Debug.Log($"ServerComunication inicializado com configuração do JSON: {serverIP}:{serverPort}");
        }
        else
        {
            Debug.LogError("ServerCommunication não encontrado! Certifique-se de que há um GameObject com ServerComunication na cena.");
        }
    }
    
    void SetupUI()
    {
        // O painel NFC não é mais necessário - usamos o feedback da ResultScreen
        if (skipNFCButton != null)
        {
            skipNFCButton.onClick.AddListener(SkipNFCAndReturnToIdle);
        }
    }
    
    public void StartNFCSession()
    {
        Debug.Log("[DEBUG] StartNFCSession chamado");
        
        if (gameResultsSent)
        {
            Debug.Log("[DEBUG] Resultados já foram enviados para este jogo - saindo");
            return;
        }
        
        Debug.Log("[DEBUG] Ativando sessão NFC...");
        
        // Ativar a espera do NFC (não precisamos mais do painel separado)
        isWaitingForNFC = true;
        Debug.Log("[DEBUG] isWaitingForNFC = true");
        
        // Mostrar feedback na ResultScreen
        ResultScreen resultScreen = Object.FindFirstObjectByType<ResultScreen>();
        if (resultScreen != null)
        {
            resultScreen.ShowNFCWaitingFeedback();
            Debug.Log("[DEBUG] Feedback mostrado na ResultScreen");
        }
        
        // Auto-return to idle after timeout
        StartCoroutine(NFCTimeout());
    }
    
    IEnumerator NFCTimeout()
    {
        yield return new WaitForSeconds(30f); // 30 segundos timeout
        
        if (isWaitingForNFC)
        {
            Debug.Log("Timeout do NFC - retornando ao menu inicial");
            SkipNFCAndReturnToIdle();
        }
    }
    
    void OnNFCConnected(string nfcId, string readerName)
    {
        Debug.Log($"[DEBUG] NFC Conectado: {nfcId} no leitor {readerName}");
        Debug.Log($"[DEBUG] Estado atual - isWaitingForNFC: {isWaitingForNFC}, gameResultsSent: {gameResultsSent}");
        
        currentNFCId = nfcId;
        currentNFCReader = readerName;
        
        if (isWaitingForNFC)
        {
            Debug.Log("[DEBUG] Processando NFC - iniciando envio de dados...");
            UpdateNFCStatusText($"Cartão detectado: {nfcId}");
            UpdateInstructionText("Enviando resultados para o servidor...");
            
            StartCoroutine(SendGameResults());
        }
        else
        {
            Debug.Log("[DEBUG] NFC conectado mas não está aguardando (isWaitingForNFC = false)");
        }
    }
    
    void OnNFCDisconnected()
    {
        Debug.Log("NFC Desconectado");
        if (isWaitingForNFC && !gameResultsSent)
        {
            UpdateNFCStatusText("Cartão removido. Aproxime novamente para salvar...");
        }
    }
    
    void OnNFCReaderConnected(string readerName)
    {
        Debug.Log($"Leitor NFC conectado: {readerName}");
        UpdateNFCStatusText("Leitor NFC conectado. Aguardando cartão...");
    }
    
    void OnNFCReaderDisconnected()
    {
        Debug.Log("Leitor NFC desconectado");
        UpdateNFCStatusText("Leitor NFC desconectado. Conecte o leitor...");
    }
    
    IEnumerator SendGameResults()
    {
        if (string.IsNullOrEmpty(currentNFCId) || DilemmaGameController.Instance == null)
        {
            UpdateNFCStatusText("Erro: Dados inválidos");
            yield return new WaitForSeconds(2f);
            FinishNFCSession();
            yield break;
        }
        
        // Criar o modelo de jogo baseado nos resultados do dilema
        GameModel gameModel = CreateGameModelFromResults();
        
        if (gameModel == null)
        {
            UpdateNFCStatusText("Erro: Não foi possível criar modelo do jogo");
            yield return new WaitForSeconds(2f);
            FinishNFCSession();
            yield break;
        }
        
        // Enviar para o servidor em background
        bool success = false;
        
        yield return StartCoroutine(SendToServerCoroutine(gameModel, (result) => {
            success = result;
        }));
        
        if (success)
        {
            UpdateNFCStatusText("Pontuação salva com sucesso!");
            UpdateInstructionText("Seus resultados foram registrados no servidor. Obrigado por participar!");
            gameResultsSent = true;
            
            // Mostrar feedback de sucesso na ResultScreen
            ResultScreen resultScreen = Object.FindFirstObjectByType<ResultScreen>();
            if (resultScreen != null)
            {
                resultScreen.ShowNFCSavedFeedback();
            }
        }
        else
        {
            UpdateNFCStatusText("Erro ao salvar pontuação");
            UpdateInstructionText("Não foi possível conectar ao servidor. Tente novamente.");
            
            // Mostrar feedback de erro na ResultScreen
            ResultScreen resultScreen = Object.FindFirstObjectByType<ResultScreen>();
            if (resultScreen != null)
            {
                resultScreen.ShowNFCErrorFeedback();
            }
        }
        
        yield return new WaitForSeconds(3f);
        FinishNFCSession();
    }
    
    IEnumerator SendToServerCoroutine(GameModel gameModel, System.Action<bool> callback)
    {
        bool success = false;
        
        var sendTask = serverCommunication.UpdateNfcInfoFromGame(gameModel);
        
        // Aguardar a conclusão da task
        while (!sendTask.IsCompleted)
        {
            yield return null;
        }
        
        if (sendTask.IsCompletedSuccessfully)
        {
            HttpStatusCode statusCode = sendTask.Result;
            success = (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created);
            Debug.Log($"Resposta do servidor: {statusCode}");
        }
        else if (sendTask.IsFaulted)
        {
            Debug.LogError($"Erro ao enviar para servidor: {sendTask.Exception?.GetBaseException().Message}");
        }
        
        callback(success);
    }
    
    GameModel CreateGameModelFromResults()
    {
        if (DilemmaGameController.Instance == null) return null;
        
        ProfileInfo finalProfile = DilemmaGameController.Instance.GetFinalProfile();
        if (finalProfile == null) return null;
        
        GameModel model = new GameModel
        {
            nfcId = currentNFCId,
            gameId = gameId
        };
        
        // Mapear pontuações baseado no perfil final
        bool isRealist = DilemmaGameController.Instance.realistAnswers > DilemmaGameController.Instance.empatheticAnswers;
        
        if (isRealist)
        {
            // O Realista - conforme imagem
            model.skill1 = realistLogicalReasoning; // Raciocínio Lógico - 9
            model.skill2 = realistSelfAwareness; // Autoconsciência - 5
            model.skill3 = realistDecisionMaking; // Tomada de decisão - 6
        }
        else
        {
            // O Empático - conforme imagem
            model.skill1 = empatheticLogicalReasoning; // Raciocínio Lógico - 8
            model.skill2 = empatheticSelfAwareness; // Autoconsciência - 6
            model.skill3 = empatheticDecisionMaking; // Tomada de decisão - 6
        }
        
        Debug.Log($"Modelo criado para {(isRealist ? "Realista" : "Empático")}: " +
                  $"Raciocínio Lógico={model.skill1}, Autoconsciência={model.skill2}, Tomada de decisão={model.skill3}");
        
        return model;
    }
    
    void FinishNFCSession()
    {
        isWaitingForNFC = false;
        
        // Retornar ao menu inicial
        if (DilemmaGameController.Instance != null)
        {
            DilemmaGameController.Instance.ResetToIdle();
        }
    }
    
    public void SkipNFCAndReturnToIdle()
    {
        Debug.Log("Usuário escolheu pular o NFC");
        StopAllCoroutines();
        FinishNFCSession();
    }
    
    public void ResetForNewGame()
    {
        Debug.Log("Resetando estado do NFC para novo jogo");
        gameResultsSent = false;
        isWaitingForNFC = false;
        currentNFCId = "";
        currentNFCReader = "";
        
        StopAllCoroutines();
        
        // Limpar feedback na ResultScreen se estiver ativa
        ResultScreen resultScreen = Object.FindFirstObjectByType<ResultScreen>();
        if (resultScreen != null)
        {
            resultScreen.ClearNFCFeedback();
        }
    }
    
    public void GetScoreMapping(bool isRealist, out int logicalReasoning, out int selfAwareness, out int decisionMaking)
    {
        if (isRealist)
        {
            logicalReasoning = realistLogicalReasoning;
            selfAwareness = realistSelfAwareness;
            decisionMaking = realistDecisionMaking;
        }
        else
        {
            logicalReasoning = empatheticLogicalReasoning;
            selfAwareness = empatheticSelfAwareness;
            decisionMaking = empatheticDecisionMaking;
        }
    }
    
    public void ReloadServerConfiguration()
    {
        Debug.Log("Recarregando configuração do servidor...");
        ServerConfig.ReloadConfiguration();
        
        var serverConfig = ServerConfig.Server;
        serverIP = serverConfig.ip;
        serverPort = serverConfig.port;
        
        if (serverCommunication != null)
        {
            serverCommunication.Ip = serverIP;
            serverCommunication.Port = serverPort;
        }
        
        Debug.Log($"Nova configuração aplicada: {serverIP}:{serverPort}");
    }
    
    void UpdateNFCStatusText(string message)
    {
        if (nfcStatusText != null)
        {
            nfcStatusText.text = message;
        }
        // Só loggar quando há uma sessão ativa
        if (isWaitingForNFC)
        {
            Debug.Log($"NFC Status: {message}");
        }
    }
    
    void UpdateInstructionText(string message)
    {
        if (nfcInstructionText != null)
        {
            nfcInstructionText.text = message;
        }
    }
    
    void OnDestroy()
    {
        if (nfcReceiver != null)
        {
            nfcReceiver.OnNFCConnected.RemoveListener(OnNFCConnected);
            nfcReceiver.OnNFCDisconnected.RemoveListener(OnNFCDisconnected);
            nfcReceiver.OnNFCReaderConnected.RemoveListener(OnNFCReaderConnected);
            nfcReceiver.OnNFCReaderDisconected.RemoveListener(OnNFCReaderDisconnected);
        }
    }
}