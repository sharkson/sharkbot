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
        private readonly ConversationService _conversationService;
        private readonly AnalyzationService _analyzationService;
        private readonly ResponseService _responseService;
        private readonly ReactionService _reactionService;
        private readonly ConversationUpdateService _covnersationUpdateService;
        private readonly UserService.UserService _userService;
        private readonly UpdateDatabasesService _updateDatabasesService;

        public BotService(ConversationService conversationService, AnalyzationService analyzationService, ResponseService responseService, ReactionService reactionService, ConversationUpdateService conversationUpdateService, UserService.UserService userService, UpdateDatabasesService updateDatabasesService)
        {
            _conversationService = conversationService;
            _analyzationService = analyzationService;
            _responseService = responseService;
            _reactionService = reactionService;
            _covnersationUpdateService = conversationUpdateService;
            _userService = userService;
            _updateDatabasesService = updateDatabasesService;
        }

        public ChatResponse ProcessChat(ChatRequest chat)
        {
            var conversation = _conversationService.GetConversation(chat);
            var analyzedConversation = _analyzationService.AnalyzeConversationAsync(conversation);
            var conversationUdpdated = _covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);

            AnalyzedChat inResponseTo = null;
            if (analyzedConversation.responses.Count() > 1)
            {
                inResponseTo = analyzedConversation.responses[analyzedConversation.responses.Count() - 2];
            }
            _userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return GetChatResponse(conversation, chat.exclusiveTypes, chat.requiredPropertyMatches, chat.excludedTypes, chat.subjectGoals);
        }

        public ChatResponse GetChatResponse(Conversation conversation, List<string> exclusiveTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            ChatResponse response;
            if ((exclusiveTypes != null && exclusiveTypes.Count > 0) || (requiredProperyMatches != null && requiredProperyMatches.Count > 0))
            {
                response = _responseService.GetResponse(conversation, exclusiveTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            }
            else
            {
                response = _responseService.GetResponse(conversation, excludedTypes, subjectGoals);
            }

            return response;
        }

        public ChatResponse GetChatReaction(Conversation conversation, List<string> exclusiveTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            ChatResponse response;
            if ((exclusiveTypes != null && exclusiveTypes.Count > 0) || (requiredProperyMatches != null && requiredProperyMatches.Count > 0))
            {
                response = _reactionService.GetReaction(conversation, exclusiveTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            }
            else
            {
                response = _reactionService.GetReaction(conversation, excludedTypes, subjectGoals);
            }

            return response;
        }
    }
}
