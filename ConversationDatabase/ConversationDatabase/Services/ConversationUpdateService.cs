using ChatModels;
using System.Collections.Concurrent;

namespace ConversationDatabase.Services
{
    public class ConversationUpdateService
    {
        private readonly ConversationSaveService _conversationSaveService;

        public ConversationUpdateService(ConversationSaveService conversationSaveService)
        {
            _conversationSaveService = conversationSaveService;
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

            _conversationSaveService.SaveConversation(conversation, type);

            return true;
        }
    }
}
