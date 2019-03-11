using System;
using System.Collections.Concurrent;

namespace ChatModels
{
    [Serializable]
    public class ConversationList
    {
        public ConcurrentDictionary<string, Conversation> conversations { get; set; }
        public string type { get; set; }
    }
}
