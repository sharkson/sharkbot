using System;

namespace ChatModels
{
    [Serializable]
    public class ConversationSubject
    {
        public string Lemmas { get; set; }
        public int OccurenceCount { get; set; }
    }
}