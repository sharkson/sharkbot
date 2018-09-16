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
        public List<string> excludedTypes { get; set; }
        public List<string> requiredProperyMatches { get; set; }
        public dynamic metadata { get; set; }
    }
}
