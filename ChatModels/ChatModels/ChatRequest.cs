using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ChatRequest
    {
        public Chat chat { get; set; }
        public string conversationName { get; set; }
        public string type { get; set; }
        public DateTime? requestTime { get; set; }
        public List<string> exclusiveTypes { get; set; }
        public List<string> requiredProperyMatches { get; set; } //TODO: test if this is working
        public dynamic metadata { get; set; }
    }
}
