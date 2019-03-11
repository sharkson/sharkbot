using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class ChatResponse
    {
        public List<string> response { get; set; }
        public double confidence { get; set; }
        public dynamic metadata { get; set; }
    }
}
