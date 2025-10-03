using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LanguageButton : MonoBehaviour
{
    [Header("Language Settings")]
    public Language targetLanguage;
    
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.SetLanguage(targetLanguage);
        }
    }
    
    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}