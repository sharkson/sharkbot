using System.Collections.Generic;
using System.Linq;
using ChatModels;

namespace ChatAnalyzer.Services
{
    public class ConversationTypeService
    {
        public bool GetConversationGroupChatType(List<AnalyzedChat> responses)
        {
            return responses.Select(r => r.chat.user).Distinct().Count() > 2;
        }
    }
}
