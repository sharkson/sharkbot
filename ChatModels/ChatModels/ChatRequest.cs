using System;

namespace ChatModels
{
    [Serializable]
    public class ChatRequest
    {
        public Chat chat { get; set; }
        public string conversationName { get; set; }
        public string type { get; set; }
        public DateTime? requestTime { get; set; }
        public dynamic metadata { get; set; }
    }
}
