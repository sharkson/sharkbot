using System;

namespace ChatModels
{
    [Serializable]
    public class AnalyzedChat
    {
        public Chat chat { get; set; }

        public NaturalLanguageData naturalLanguageData { get; set; }

        public string botName { get; set; }
    }
}
