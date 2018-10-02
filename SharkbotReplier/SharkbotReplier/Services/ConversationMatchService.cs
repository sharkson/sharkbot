using ChatModels;
using ConversationMatcher.Services;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ConversationMatchService
    {
        private BestMatchService bestMatchService;

        public ConversationMatchService()
        {
            bestMatchService = new BestMatchService();
        }

        public MatchChat GetConversationMatch(Conversation conversation, List<string> excludedTypes, List<string> subjectGoals)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase.Where(cl => !excludedTypes.Any(t => cl.type == t)).ToList();
            var conversationMatchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
            return bestMatchService.GetBestMatch(conversationMatchRequest.conversation, conversationMatchRequest.conversationLists, subjectGoals);
        }

        public MatchChat GetConversationMatch(Conversation conversation, List<string> requiredTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase.Where(cl => !excludedTypes.Any(t => cl.type == t)).ToList();
            if (requiredTypes.Count > 0)
            {
                conversationLists = conversationLists.Where(cl => requiredTypes.Any(t => cl.type == t)).ToList();
            }
            
            if(requiredProperyMatches.Count > 0)
            {
                var userData = UserDatabase.UserDatabase.userDatabase.Where(user => user.userName == conversation.responses.Last().chat.user && requiredProperyMatches.All(requiredProperty => user.derivedProperties.Any(dp => dp.name == requiredProperty))).FirstOrDefault();
                var botData = UserDatabase.UserDatabase.userDatabase.Where(user => user.userName == conversation.responses.Last().chat.botName && requiredProperyMatches.All(requiredProperty => user.derivedProperties.Any(dp => dp.name == requiredProperty))).FirstOrDefault();
                if (userData != null)
                {
                    var propertiesToMatch = userData.derivedProperties.Where(dp => requiredProperyMatches.Contains(dp.name));
                    var matchingUsers = UserDatabase.UserDatabase.userDatabase.Where(user => propertiesToMatch.All(ptm => user.derivedProperties.Any(dp => dp.name == ptm.name && dp.value == ptm.value))).ToList();

                    var botPropertiesToMatch = botData.derivedProperties.Where(dp => requiredProperyMatches.Contains(dp.name));
                    var usersMatchingBot = UserDatabase.UserDatabase.userDatabase.Where(user => botPropertiesToMatch.All(ptm => user.derivedProperties.Any(dp => dp.name == ptm.name && dp.value == ptm.value))).ToList();                  

                    var matchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
                    return bestMatchService.GetBestMatch(matchRequest.conversation, matchRequest.conversationLists, subjectGoals, matchingUsers, usersMatchingBot);
                }             
            }
            var conversationMatchRequest = new ConversationMatchRequest { conversation = conversation, conversationLists = conversationLists };
            return bestMatchService.GetBestMatch(conversationMatchRequest.conversation, conversationMatchRequest.conversationLists, subjectGoals);
        }
    }
}
