using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ConversationRequest
    {
        public List<AnalyzedChat> responses { get; set; }
        public string name { get; set; }
        public string type { get; set; }    
        public DateTime requestTime { get; set; }
        public dynamic metadata { get; set; }
    }
}
