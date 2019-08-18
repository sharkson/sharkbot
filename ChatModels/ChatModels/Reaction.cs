using System;

namespace ChatModels
{
    [Serializable]
    public class Reaction
    {
        public string user { get; set; }
        public string reaction { get; set; }
        public long time { get; set; }
    }
}
