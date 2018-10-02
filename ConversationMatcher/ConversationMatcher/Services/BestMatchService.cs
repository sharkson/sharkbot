using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ConversationMatcher.Services
{
    public class BestMatchService
    {
        private MatchService matchService;

        public BestMatchService()
        {
            matchService = new MatchService();
        }

        public MatchChat GetBestMatch(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals)
        {
            var bestMatch = new MatchChat { matchConfidence = 0 };

            var conversationMatchLists = matchService.GetConversationMatchLists(targetConversation, conversationLists, subjectGoals);
            foreach (var conversationMatchList in conversationMatchLists)
            {
                foreach (var conversation in conversationMatchList.matchConversations)
                {
                    for (var index = 0; index < conversation.responses.Count; index++)
                    {
                        if (conversation.responses[index].matchConfidence > bestMatch.matchConfidence)
                        {
                            bestMatch.matchConfidence = conversation.responses[index].matchConfidence;
                            bestMatch.analyzedChat = conversation.responses[index].analyzedChat;
                            if (index + 1 < conversation.responses.Count)
                            {
                                bestMatch.responseChat = matchService.GetResponseChat(conversation.responses, index);
                            }
                        }
                    }
                }
            }

            return bestMatch;
        }

        public MatchChat GetBestMatch(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals, List<UserData> matchingUsers, List<UserData> usersMatchingBot)
        {
            var bestMatch = new MatchChat { matchConfidence = 0 };

            var conversationMatchLists = matchService.GetConversationMatchLists(targetConversation, conversationLists, subjectGoals);
            foreach (var conversationMatchList in conversationMatchLists)
            {
                foreach (var conversation in conversationMatchList.matchConversations)
                {
                    for (var index = 0; index < conversation.responses.Count; index++)
                    {
                        if (conversation.responses[index].matchConfidence > bestMatch.matchConfidence && userMatch(conversation, index, matchingUsers) && userMatch(conversation, index + 1, usersMatchingBot))
                        {
                            bestMatch.matchConfidence = conversation.responses[index].matchConfidence;
                            bestMatch.analyzedChat = conversation.responses[index].analyzedChat;
                            if (index + 1 < conversation.responses.Count)
                            {
                                bestMatch.responseChat = matchService.GetResponseChat(conversation.responses, index);
                            }
                        }
                    }
                }
            }

            return bestMatch;
        }

        private bool userMatch(MatchConversation conversation, int index, List<UserData> matchingUsers)
        {
            return matchingUsers.Any(user => user.userName == conversation.responses[index].analyzedChat.chat.user);
        }
    }
}
