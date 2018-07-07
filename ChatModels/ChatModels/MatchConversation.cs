using System.Collections.Generic;

namespace ChatModels
{
    public class MatchConversation
    {
        public string name { get; set; }
        public List<MatchChat> responses { get; set; }
    }
}
