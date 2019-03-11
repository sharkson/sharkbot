using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class Conversation
    {
        public string name { get; set; }
        public List<AnalyzedChat> responses { get; set; }
        public List<ConversationSubject> subjects { get; set; }
        public string analyzationVersion { get; set; }
        public bool groupChat { get; set; }
        public ReadingLevel readingLevel { get; set; }
    }
}