using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ConversationSubject
    {
        public List<string> subjectWords { get; set; }
        public int occurenceCount { get; set; }
    }
}