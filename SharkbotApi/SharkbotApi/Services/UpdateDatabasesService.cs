using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using System.Linq;

namespace SharkbotApi.Services
{
    public class UpdateDatabasesService
    {
        private ConversationService conversationService;
        private AnalyzationService analyzationService;
        private ConversationUpdateService covnersationUpdateService;
        private UserService.UserService userService;

        public UpdateDatabasesService()
        {
            conversationService = new ConversationService();
            analyzationService = new AnalyzationService();
            covnersationUpdateService = new ConversationUpdateService();
            userService = new UserService.UserService();
        }

        public bool UpdateDatabases(ChatRequest chat)
        {
            var conversation = conversationService.GetConversation(chat);
            AnalyzedChat inResponseTo = null;
            if (conversation.responses.Count() > 1)
            {
                inResponseTo = conversation.responses[conversation.responses.Count() - 2];
            }
            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);
            userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return conversationUdpdated;
        }

        public bool UpdateDatabases(ConversationRequest conversationRequest)
        {
            var conversation = new Conversation
            {
                name = conversationRequest.name,
                responses = conversationRequest.responses
            };

            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, conversationRequest.type);

            AnalyzedChat previousChat = null;
            foreach (var analyziedChat in analyzedConversation.responses)
            {
                userService.UpdateUsers(analyziedChat, previousChat);
                previousChat = analyziedChat;
            }

            return conversationUdpdated;
        }
    }
}
