using UnityEngine;

public class LanguageSwitcher : MonoBehaviour
{
    [Header("Portuguese Objects")]
    [Tooltip("GameObjects que ficam ativos quando o idioma está em português")]
    public GameObject[] portugueseObjects;

    [Header("English Objects")]
    [Tooltip("GameObjects que ficam ativos quando o idioma está em inglês")]
    public GameObject[] englishObjects;

    void OnEnable()
    {
        LanguageManager.OnLanguageChanged += UpdateLanguageObjects;
    }

    void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= UpdateLanguageObjects;
    }

    void Start()
    {
        UpdateLanguageObjects();
    }

    void UpdateLanguageObjects()
    {
        if (LanguageManager.Instance == null) return;

        bool isPortuguese = LanguageManager.Instance.IsPortuguese();

        foreach (GameObject obj in portugueseObjects)
        {
            if (obj != null)
                obj.SetActive(isPortuguese);
        }

        foreach (GameObject obj in englishObjects)
        {
            if (obj != null)
                obj.SetActive(!isPortuguese);
        }
    }
}
