using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class Chat
    {
        public Chat()
        {
            time = 0;
            reactions = new List<Reaction>();
        }

        public string user { get; set; }
        public string message { get; set; }
        public long time { get; set; }
        public string botName { get; set; }
        public List<Reaction> reactions { get; set; }
    }
}