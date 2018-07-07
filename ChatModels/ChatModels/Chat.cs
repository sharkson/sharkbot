using System;

namespace ChatModels
{
    [Serializable]
    public class Chat
    {
        public Chat()
        {
            time = 0;
        }

        public string user { get; set; }
        public string message { get; set; }
        public long time { get; set; }
        public string botName { get; set; }
    }
}