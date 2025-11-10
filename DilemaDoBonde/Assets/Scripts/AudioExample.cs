using UnityEngine;

public class AudioExample : MonoBehaviour
{
    void Start()
    {
        PlayExampleSound();
    }
    
    void PlayExampleSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStartButtonSound();
        }
        else
        {
            Debug.LogWarning("AudioManager não encontrado na cena!");
        }
    }
    
    public void OnButtonClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayChoiceSound();
        }
    }
    
    public void OnNFCScanned()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayNFCReadSound();
        }
    }
    
    public void OnGameFinished()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameEndSound();
        }
    }
}
