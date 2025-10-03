namespace RealGames
{
    [System.Serializable]
    public class DilemmaConfig
    {
        public int timeoutSeconds;
        public float choiceDisplayTime;
        public float resultDisplayTime;
        public ProfilesData profiles;
        public DilemmaData[] dilemmas;
    }

    [System.Serializable]
    public class ProfilesData
    {
        public ProfileInfo realist;
        public ProfileInfo empathetic;
    }

    [System.Serializable]
    public class ProfileInfo
    {
        public LocalizedText name;
        public LocalizedText description;
    }

    [System.Serializable]
    public class DilemmaData
    {
        public int id;
        public LocalizedText title;
        public LocalizedText description;
        public LocalizedText question;
        public DilemmaOption optionA;
        public DilemmaOption optionB;
    }

    [System.Serializable]
    public class DilemmaOption
    {
        public LocalizedText text;
        public string type;
    }
    
    [System.Serializable]
    public class LocalizedText
    {
        public string pt;
        public string en;
        
        public string GetText(Language language)
        {
            return language == Language.Portuguese ? pt : en;
        }
        
        public string GetText()
        {
            if (LanguageManager.Instance != null)
            {
                return GetText(LanguageManager.Instance.currentLanguage);
            }
            return pt; // Default to Portuguese
        }
    }
}