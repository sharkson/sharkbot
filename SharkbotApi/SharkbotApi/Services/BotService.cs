using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using SharkbotReplier.Services;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotApi.Services
{
    public class BotService
    {
        private ConversationService conversationService;
        private AnalyzationService analyzationService;
        private ResponseService responseService;
        private ConversationUpdateService covnersationUpdateService;
        private UserService.UserService userService;
        private UpdateDatabasesService updateDatabasesService;

        public BotService()
        {
            conversationService = new ConversationService();
            analyzationService = new AnalyzationService();
            responseService = new ResponseService();
            covnersationUpdateService = new ConversationUpdateService();
            userService = new UserService.UserService();
            updateDatabasesService = new UpdateDatabasesService();
        }

        public ChatResponse ProcessChat(ChatRequest chat)
        {
            var conversation = conversationService.GetConversation(chat);
            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);

            AnalyzedChat inResponseTo = null;
            if (analyzedConversation.responses.Count() > 1)
            {
                inResponseTo = analyzedConversation.responses[analyzedConversation.responses.Count() - 2];
            }
            userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return GetChatResponse(conversation, chat.exclusiveTypes, chat.requiredProperyMatches, chat.excludedTypes, chat.subjectGoals);
        }

        public ChatResponse GetChatResponse(Conversation conversation, List<string> exclusiveTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            ChatResponse response;
            if ((exclusiveTypes != null && exclusiveTypes.Count > 0) || (requiredProperyMatches != null && requiredProperyMatches.Count > 0))
            {
                response = responseService.GetResponse(conversation, exclusiveTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            }
            else
            {
                response = responseService.GetResponse(conversation, excludedTypes, subjectGoals);
            }

            return response;
        }
    }
}
