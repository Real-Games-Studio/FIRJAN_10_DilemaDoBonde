using System.Collections;
using UnityEngine;
using RealGames;
using System.IO;

public class DilemmaGameController : MonoBehaviour
{
    [Header("Game Configuration")]
    public DilemmaConfig dilemmaConfig;
    
    [Header("Game State")]
    public int currentDilemmaIndex = 0;
    public int realistAnswers = 0;
    public int empatheticAnswers = 0;
    
    [Header("Screen Names")]
    public string idleScreenName = "IdleScreen";
    public string dilemmaScreenName = "DilemmaScreen";
    public string choiceScreenName = "ChoiceScreen";
    public string resultScreenName = "ResultScreen";
    
    [Header("Timing")]
    [SerializeField] private float choiceDisplayTime = 5f; // Default fallback value
    [SerializeField] private float timeoutTime = 60f; // Default fallback value
    
    private float currentTimer = 0f;
    private bool isGameActive = false;
    
    public static DilemmaGameController Instance { get; private set; }
    
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
        LoadDilemmaConfiguration();
        ResetToIdle();
    }
    
    void Update()
    {
        HandleInput();
        HandleTimeout();
    }
    
    void LoadDilemmaConfiguration()
    {
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
        dilemmaConfig = JsonLoader.LoadDilemmaConfig(configPath);
        
        if (dilemmaConfig != null)
        {
            timeoutTime = dilemmaConfig.timeoutSeconds;
            choiceDisplayTime = dilemmaConfig.choiceDisplayTime;
            Debug.Log($"Dilemma config loaded successfully. Timeout: {timeoutTime}s, Choice Display: {choiceDisplayTime}s, Result Display: {dilemmaConfig.resultDisplayTime}s, Dilemmas: {dilemmaConfig.dilemmas.Length}");
        }
        else
        {
            Debug.LogError("Failed to load dilemma configuration!");
        }
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNumberInput(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnNumberInput(2);
        }
    }
    
    void HandleTimeout()
    {
        if (isGameActive)
        {
            currentTimer += Time.deltaTime;
            
            if (currentTimer >= timeoutTime)
            {
                ResetToIdle();
            }
        }
    }
    
    void OnNumberInput(int number)
    {
        currentTimer = 0f;
        
        string currentScreen = ScreenManager.currentScreenName;
        
        if (currentScreen == idleScreenName)
        {
            if (number == 1 || number == 2)
            {
                StartGame();
            }
        }
        else if (currentScreen == dilemmaScreenName)
        {
            if (number == 1)
            {
                AnswerDilemma(true); // Option A
            }
            else if (number == 2)
            {
                AnswerDilemma(false); // Option B
            }
        }
    }
    
    public void StartGame()
    {
        Debug.Log("Starting Dilemma Game");
        currentDilemmaIndex = 0;
        realistAnswers = 0;
        empatheticAnswers = 0;
        isGameActive = true;
        currentTimer = 0f;
        
        ShowCurrentDilemma();
    }
    
    void ShowCurrentDilemma()
    {
        if (currentDilemmaIndex < dilemmaConfig.dilemmas.Length)
        {
            ScreenManager.SetCallScreen(dilemmaScreenName);
            Debug.Log($"Showing dilemma {currentDilemmaIndex + 1}: {dilemmaConfig.dilemmas[currentDilemmaIndex].title}");
        }
        else
        {
            ShowFinalResult();
        }
    }
    
    public void AnswerDilemma(bool chooseOptionA)
    {
        DilemmaData currentDilemma = dilemmaConfig.dilemmas[currentDilemmaIndex];
        string chosenType;
        
        if (chooseOptionA)
        {
            chosenType = currentDilemma.optionA.type;
            Debug.Log($"Player chose option A: {currentDilemma.optionA.text}");
        }
        else
        {
            chosenType = currentDilemma.optionB.type;
            Debug.Log($"Player chose option B: {currentDilemma.optionB.text}");
        }
        
        if (chosenType == "realist")
        {
            realistAnswers++;
        }
        else if (chosenType == "empathetic")
        {
            empatheticAnswers++;
        }
        
        ShowChoiceConfirmation(chooseOptionA);
    }
    
    void ShowChoiceConfirmation(bool chooseOptionA)
    {
        ScreenManager.SetCallScreen(choiceScreenName);
        StartCoroutine(WaitAndProceed());
    }
    
    IEnumerator WaitAndProceed()
    {
        yield return new WaitForSeconds(choiceDisplayTime);
        
        currentDilemmaIndex++;
        ShowCurrentDilemma();
    }
    
    void ShowFinalResult()
    {
        ScreenManager.SetCallScreen(resultScreenName);
        isGameActive = false;
        
        string profileType = realistAnswers > empatheticAnswers ? "realist" : "empathetic";
        Debug.Log($"Game finished. Realist: {realistAnswers}, Empathetic: {empatheticAnswers}, Result: {profileType}");
    }
    
    public void ResetToIdle()
    {
        Debug.Log("Returning to idle screen");
        isGameActive = false;
        currentTimer = 0f;
        currentDilemmaIndex = 0;
        realistAnswers = 0;
        empatheticAnswers = 0;
        
        ScreenManager.SetCallScreen(idleScreenName);
    }
    
    public DilemmaData GetCurrentDilemma()
    {
        if (currentDilemmaIndex < dilemmaConfig.dilemmas.Length)
        {
            return dilemmaConfig.dilemmas[currentDilemmaIndex];
        }
        return null;
    }
    
    public ProfileInfo GetFinalProfile()
    {
        if (realistAnswers > empatheticAnswers)
        {
            return dilemmaConfig.profiles.realist;
        }
        else
        {
            return dilemmaConfig.profiles.empathetic;
        }
    }
    
    public string GetLastChoice()
    {
        if (currentDilemmaIndex > 0)
        {
            DilemmaData lastDilemma = dilemmaConfig.dilemmas[currentDilemmaIndex - 1];
            // This would need to be stored during the choice - for now return a placeholder
            return "Sua escolha foi registrada";
        }
        return "";
    }
    
    public float GetResultDisplayTime()
    {
        return dilemmaConfig != null ? dilemmaConfig.resultDisplayTime : 10f;
    }
}