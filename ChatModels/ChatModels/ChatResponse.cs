using System;

namespace ChatModels
{
    [Serializable]
    public class ChatResponse
    {
        public string response { get; set; }
        public double confidence { get; set; }
        public dynamic metadata { get; set; }
    }
}
