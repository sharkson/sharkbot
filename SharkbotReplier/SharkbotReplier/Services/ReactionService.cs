using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ReactionService
    {
        private readonly ConversationReactionMatchService _conversationMatchService;

        public ReactionService(ConversationReactionMatchService conversationMatchService)
        {
            _conversationMatchService = conversationMatchService;
        }

        public ChatResponse GetReaction(Conversation analyzedConversation, List<string> excludedTypes, List<string> subjectGoals)
        {
            if (analyzedConversation.responses.Count == 0)
            {
                return new ChatResponse() { confidence = 0, response = new List<string>() };
            }

            var conversationChatResponse = _conversationMatchService.GetConversationMatch(analyzedConversation, excludedTypes, subjectGoals);
 
            var matchChat = conversationChatResponse;

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = GetResponse(matchChat)
            };

            return chatResponse;
        }

        public ChatResponse GetReaction(Conversation analyzedConversation, List<string> requiredTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            //TODO: change type to list of types, pass that in. if it's empty do any
            var conversationChatResponse = _conversationMatchService.GetConversationMatch(analyzedConversation, requiredTypes, requiredProperyMatches, excludedTypes, subjectGoals);

            var matchChat = conversationChatResponse;

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = GetResponse(matchChat)
            };

            //TODO: alter reply to match sophistication

            return chatResponse;
        }

        public List<string> GetResponse(MatchChat match)
        {
            var responses = new List<string>();
            foreach (var matchChat in match.responseChat)
            {
                responses.Add(matchChat.chat.message);
            }

            return responses;
        }
    }
}
