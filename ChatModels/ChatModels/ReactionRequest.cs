using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ReactionRequest
    {
        public Reaction reaction { get; set; }
        public Chat chat { get; set; }
        public string conversationName { get; set; }
        public string type { get; set; }
        public DateTime? requestTime { get; set; }
        public List<string> exclusiveTypes { get; set; }
        public List<string> excludedTypes { get; set; }
        public List<string> requiredProperyMatches { get; set; }
        public List<string> subjectGoals { get; set; }
        public dynamic metadata { get; set; }
    }
}
