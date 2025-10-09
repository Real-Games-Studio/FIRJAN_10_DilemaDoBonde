using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class ServerConfiguration
{
    public string ip = "127.0.0.1";
    public int port = 8080;
}

[Serializable]
public class GameConfiguration
{
    public int timeoutSeconds = 60;
    public int choiceDisplayTime = 5;
    public int resultDisplayTime = 10;
    public ServerConfiguration server = new ServerConfiguration();
}

public static class ServerConfig
{
    private static GameConfiguration _config;
    
    public static ServerConfiguration Server
    {
        get
        {
            LoadConfigIfNeeded();
            return _config.server;
        }
    }
    
    public static GameConfiguration Config
    {
        get
        {
            LoadConfigIfNeeded();
            return _config;
        }
    }
    
    private static void LoadConfigIfNeeded()
    {
        if (_config == null)
        {
            LoadConfiguration();
        }
    }
    
    private static void LoadConfiguration()
    {
        try
        {
            string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
            
            if (File.Exists(configPath))
            {
                string jsonContent = File.ReadAllText(configPath);
                _config = JsonConvert.DeserializeObject<GameConfiguration>(jsonContent);
                
                if (_config.server == null)
                {
                    _config.server = new ServerConfiguration();
                }
                
                Debug.Log($"Configuração carregada: Servidor {_config.server.ip}:{_config.server.port}");
            }
            else
            {
                Debug.LogWarning($"Arquivo config.json não encontrado em: {configPath}");
                _config = new GameConfiguration();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao carregar configuração: {ex.Message}");
            _config = new GameConfiguration();
        }
    }
    
    public static void ReloadConfiguration()
    {
        _config = null;
        LoadConfiguration();
    }
}