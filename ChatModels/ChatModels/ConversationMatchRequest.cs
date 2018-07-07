using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ConversationMatchRequest
    {
        public Conversation conversation { get; set; }
        public List<ConversationList> conversationLists { get; set; }
    }
}
