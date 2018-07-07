using ChatModels;
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
            if (ConversationDatabase.conversationDatabase.Any(cl => cl.type == type))
            {
                if (ConversationDatabase.conversationDatabase.Where(cl => cl.type == type).First().conversations.Any(c => c.name == conversation.name))
                {
                    ConversationDatabase.conversationDatabase.Where(cl => cl.type == type).First().conversations.Remove(ConversationDatabase.conversationDatabase.Where(cl => cl.type == type).First().conversations.Where(c => c.name == conversation.name).First());
                }
                ConversationDatabase.conversationDatabase.Where(cl => cl.type == type).First().conversations.Add(conversation);
            }
            else
            {
                var converstaions = new List<Conversation>();
                converstaions.Add(conversation);
                var list = new ConversationList
                {
                    type = type,
                    conversations = converstaions
                };
                ConversationDatabase.conversationDatabase.Add(list);
            }

            conversationSaveService.SaveConversation(conversation, type);

            return true;
        }
    }
}
