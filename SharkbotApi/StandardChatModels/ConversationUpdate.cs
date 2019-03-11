using System;

namespace ChatModels
{
    [Serializable]
    public class ConversationUpdate
    {
        public string type { get; set; }
        public Conversation conversation { get; set; }
    }
}
