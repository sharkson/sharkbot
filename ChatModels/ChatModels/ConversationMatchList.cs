using System.Collections.Concurrent;

namespace ChatModels
{
    public class ConversationMatchList
    {
        public ConcurrentBag<MatchConversation> matchConversations { get; set; }
        public string type { get; set; }
    }
}
