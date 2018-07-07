using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModels
{
    [Serializable]
    public class ConversationList
    {
        public List<Conversation> conversations { get; set; }
        public string type { get; set; }
    }
}
