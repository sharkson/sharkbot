using ChatModels;
using ConversationMatcher.Services;
using System.Collections.Generic;
using System.Linq;

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

        public MatchChat GetConversationMatch(Conversation conversation, List<string> types, List<string> requiredProperyMatches)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;
            if (types.Count > 0)
            {
                conversationLists = conversationLists.Where(cl => types.Any(t => cl.type == t)).ToList();
            }
            
            if(requiredProperyMatches.Count > 0)
            {
                var userData = UserDatabase.UserDatabase.userDatabase.Where(user => user.userName == conversation.responses.Last().chat.user && requiredProperyMatches.All(requiredProperty => user.derivedProperties.Any(dp => dp.name == requiredProperty))).FirstOrDefault();
                if(userData != null)
                {
                    var propertiesToMatch = userData.derivedProperties.Where(dp => requiredProperyMatches.Contains(dp.name));
                    var matchingUsers = UserDatabase.UserDatabase.userDatabase.Where(user => propertiesToMatch.All(ptm => user.derivedProperties.Any(dp => dp.name == ptm.name && dp.value == ptm.value))).ToList();

                    var matchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
                    return matchService.GetBestMatch(matchRequest.conversation, matchRequest.conversationLists, matchingUsers);
                }             
            }
            var conversationMatchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
            return matchService.GetBestMatch(conversationMatchRequest.conversation, conversationMatchRequest.conversationLists);
        }

        public static bool ContainsAll<T>(IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.All(value => source.Contains(value));
        }
    }
}
