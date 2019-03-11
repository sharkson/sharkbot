using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using System.Linq;

namespace SharkbotApi.Services
{
    public class UpdateDatabasesService
    {
        private readonly ConversationService _conversationService;
        private readonly AnalyzationService _analyzationService;
        private readonly ConversationUpdateService _covnersationUpdateService;
        private readonly UserService.UserService _userService;

        public UpdateDatabasesService(ConversationService conversationService, AnalyzationService analyzationService, ConversationUpdateService conversationUpdateService, UserService.UserService userService)
        {
            _conversationService = conversationService;
            _analyzationService = analyzationService;
            _covnersationUpdateService = conversationUpdateService;
            _userService = userService;
        }

        public bool UpdateDatabases(ChatRequest chat)
        {
            var conversation = _conversationService.GetConversation(chat);
            AnalyzedChat inResponseTo = null;
            if (conversation.responses.Count() > 1)
            {
                inResponseTo = conversation.responses[conversation.responses.Count() - 2];
            }
            var analyzedConversation = _analyzationService.AnalyzeConversationAsync(conversation);
            var conversationUdpdated = _covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);
            _userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return conversationUdpdated;
        }

        public bool UpdateDatabases(ConversationRequest conversationRequest)
        {
            var conversation = new Conversation
            {
                name = conversationRequest.name,
                responses = conversationRequest.responses
            };

            var analyzedConversation = _analyzationService.AnalyzeConversationAsync(conversation);
            var conversationUdpdated = _covnersationUpdateService.UpdateConversation(analyzedConversation, conversationRequest.type);

            AnalyzedChat previousChat = null;
            foreach (var analyziedChat in analyzedConversation.responses)
            {
                _userService.UpdateUsers(analyziedChat, previousChat);
                previousChat = analyziedChat;
            }

            return conversationUdpdated;
        }
    }
}
