using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ConversationMatcher.Services
{
    public class BestReactionMatchService
    {
        private readonly MatchService _matchService;

        public BestReactionMatchService(MatchService matchService)
        {
            _matchService = matchService;
        }

        public MatchChat GetBestMatch(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals)
        {
            var bestMatch = new MatchChat { matchConfidence = 0 };

            var conversationMatchLists = _matchService.GetConversationMatchLists(targetConversation, conversationLists, subjectGoals);
            foreach (var conversationMatchList in conversationMatchLists)
            {
                foreach (var conversation in conversationMatchList.matchConversations)
                {
                    for (var index = 0; index < conversation.responses.Count; index++)
                    {
                        if (conversation.responses[index].matchConfidence > bestMatch.matchConfidence && conversation.responses[index].analyzedChat.chat.reactions.Count > 0)
                        {
                            bestMatch.matchConfidence = conversation.responses[index].matchConfidence;
                            bestMatch.analyzedChat = conversation.responses[index].analyzedChat;
                            bestMatch.responseChat = getAnalyzedChatsFromReactions(conversation.responses[index].analyzedChat.chat.reactions);
                        }
                    }
                }
            }

            return bestMatch;
        }

        public MatchChat GetBestMatch(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals, List<UserData> matchingUsers, List<UserData> usersMatchingBot)
        {
            var bestMatch = new MatchChat { matchConfidence = 0 };

            var conversationMatchLists = _matchService.GetConversationMatchLists(targetConversation, conversationLists, subjectGoals);
            foreach (var conversationMatchList in conversationMatchLists)
            {
                foreach (var conversation in conversationMatchList.matchConversations)
                {
                    for (var index = 0; index < conversation.responses.Count; index++)
                    {
                        if (conversation.responses[index].matchConfidence > bestMatch.matchConfidence && userMatch(conversation, index, matchingUsers) && userMatch(conversation, index + 1, usersMatchingBot) && conversation.responses[index].analyzedChat.chat.reactions.Count > 0)
                        {
                            bestMatch.matchConfidence = conversation.responses[index].matchConfidence;
                            bestMatch.analyzedChat = conversation.responses[index].analyzedChat;
                            bestMatch.responseChat = getAnalyzedChatsFromReactions(conversation.responses[index].analyzedChat.chat.reactions);
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

        private List<AnalyzedChat> getAnalyzedChatsFromReactions(List<Reaction> reactions)
        {
            var analyzedChats = new List<AnalyzedChat>();
            foreach(var reaction in reactions)
            {
                var analyzedChat = new AnalyzedChat();
                analyzedChat.chat = new Chat();
                analyzedChat.chat.message = reaction.reaction;
                analyzedChats.Add(analyzedChat);
            }
            return analyzedChats;
        }
    }
}
