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
        public string name;
        public string description;
    }

    [System.Serializable]
    public class DilemmaData
    {
        public int id;
        public string title;
        public string description;
        public string question;
        public DilemmaOption optionA;
        public DilemmaOption optionB;
    }

    [System.Serializable]
    public class DilemmaOption
    {
        public string text;
        public string type;
    }
}