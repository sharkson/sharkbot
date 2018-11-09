using ChatModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ConversationDatabase.Services
{
    public class ConversationUpdateService
    {
        private ConversationSaveService conversationSaveService;

        public ConversationUpdateService()
        {
            conversationSaveService = new ConversationSaveService();
        }

        public bool UpdateConversation(Conversation conversation, string type)
        {
            if (ConversationDatabase.conversationDatabase.ContainsKey(type))
            {
                ConversationDatabase.conversationDatabase[type].conversations[conversation.name] = conversation;
            }
            else
            {
                var converstaions = new ConcurrentDictionary<string, Conversation>();
                converstaions[conversation.name] = conversation;
                var list = new ConversationList
                {
                    type = type,
                    conversations = converstaions
                };
                ConversationDatabase.conversationDatabase[type] = list;
            }

            conversationSaveService.SaveConversation(conversation, type);

            return true;
        }
    }
}
