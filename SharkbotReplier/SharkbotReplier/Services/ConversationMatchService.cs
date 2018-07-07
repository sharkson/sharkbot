using ChatModels;
using ConversationMatcher.Services;

namespace SharkbotReplier.Services
{
    public class ConversationMatchService
    {
        private MatchService matchService;

        public ConversationMatchService()
        {
            matchService = new MatchService();
        }

        public MatchChat GetConversationMatch(Conversation conversation)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;
            var conversationMatchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
            return matchService.GetBestMatch(conversationMatchRequest.conversation, conversationMatchRequest.conversationLists);
        }
    }
}
