using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerenciador central de áudio do jogo.
/// Sistema integrado automaticamente para tocar sons em:
/// - Leitura NFC (NFCGameManager)
/// - Escolhas A/B (DilemmaGameController)
/// - Botão Iniciar (DilemmaGameController)
/// - Fim de Jogo (DilemmaGameController)
/// 
/// Para usar: GameObject > Audio > Setup AudioManager
/// Para verificar: Tools > Audio > Verificar Configuração de Áudio
/// 
/// Documentação: Veja LEIA-ME_AUDIO.md ou GUIA_RAPIDO_3_PASSOS.txt
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip nfcReadClip;
    [SerializeField] private AudioClip choiceClip;
    [SerializeField] private AudioClip startButtonClip;
    [SerializeField] private AudioClip gameEndClip;
    [SerializeField] private AudioClip backgroundMusicClip;
    
    private Dictionary<string, AudioClip> audioClips;
    private const string AUDIO_FOLDER_PATH = "Audio/SFX";
    private const string MUSIC_FOLDER_PATH = "Audio/Music";
    
    private const string NFC_READ_SOUND = "nfc_read";
    private const string CHOICE_SOUND = "choice";
    private const string START_BUTTON_SOUND = "start_button";
    private const string GAME_END_SOUND = "game_end";
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            LoadAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        UpdateVolumes();
    }
    
    void LoadAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        
        if (nfcReadClip != null) audioClips[NFC_READ_SOUND] = nfcReadClip;
        if (choiceClip != null) audioClips[CHOICE_SOUND] = choiceClip;
        if (startButtonClip != null) audioClips[START_BUTTON_SOUND] = startButtonClip;
        if (gameEndClip != null) audioClips[GAME_END_SOUND] = gameEndClip;
        
        TryLoadFromResources();
    }
    
    void TryLoadFromResources()
    {
        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>(AUDIO_FOLDER_PATH);
        foreach (AudioClip clip in sfxClips)
        {
            if (!audioClips.ContainsKey(clip.name))
            {
                audioClips[clip.name] = clip;
                Debug.Log($"Loaded audio clip from Resources: {clip.name}");
            }
        }
        
        if (backgroundMusicClip == null)
        {
            AudioClip[] musicClips = Resources.LoadAll<AudioClip>(MUSIC_FOLDER_PATH);
            if (musicClips.Length > 0)
            {
                backgroundMusicClip = musicClips[0];
            }
        }
    }
    
    void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = masterVolume * musicVolume;
        }
        
        if (sfxSource != null)
        {
            sfxSource.volume = masterVolume * sfxVolume;
        }
    }
    
    public void PlayNFCReadSound()
    {
        PlaySound(NFC_READ_SOUND);
    }
    
    public void PlayChoiceSound()
    {
        PlaySound(CHOICE_SOUND);
    }
    
    public void PlayStartButtonSound()
    {
        PlaySound(START_BUTTON_SOUND);
    }
    
    public void PlayGameEndSound()
    {
        PlaySound(GAME_END_SOUND);
    }
    
    public void PlaySound(string soundName)
    {
        if (audioClips.ContainsKey(soundName))
        {
            sfxSource.PlayOneShot(audioClips[soundName]);
        }
        else
        {
            Debug.LogWarning($"Audio clip '{soundName}' not found!");
        }
    }
    
    public void PlaySoundWithVolume(string soundName, float volumeScale)
    {
        if (audioClips.ContainsKey(soundName))
        {
            sfxSource.PlayOneShot(audioClips[soundName], volumeScale);
        }
        else
        {
            Debug.LogWarning($"Audio clip '{soundName}' not found!");
        }
    }
    
    public void PlayBackgroundMusic()
    {
        if (backgroundMusicClip != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusicClip;
            musicSource.Play();
        }
    }
    
    public void StopBackgroundMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    public void MuteAll(bool mute)
    {
        musicSource.mute = mute;
        sfxSource.mute = mute;
    }
    
    public void MuteMusic(bool mute)
    {
        musicSource.mute = mute;
    }
    
    public void MuteSFX(bool mute)
    {
        sfxSource.mute = mute;
    }
}
