using ChatModels;
using ConversationSteerService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConversationMatcher.Services
{
    public class MatchService
    {
        private readonly SubjectConfidenceService _subjectConfidenceService;
        private readonly MatchConfidenceService _matchConfidenceService;
        private readonly GroupChatConfidenceService _groupChatConfidenceService;
        private readonly UniqueConfidenceService _uniqueConfidenceService;
        private readonly ReadingLevelConfidenceService _readingLevelConfidenceService;
        private readonly ConversationPathService _conversationPathService;

        public MatchService(SubjectConfidenceService subjectConfidenceService, MatchConfidenceService matchConfidenceService, GroupChatConfidenceService groupChatConfidenceService, UniqueConfidenceService uniqueConfidenceService, ReadingLevelConfidenceService readingLevelConfidenceService, ConversationPathService conversationPathService)
        {
            _subjectConfidenceService = subjectConfidenceService;
            _matchConfidenceService = matchConfidenceService;
            _groupChatConfidenceService = groupChatConfidenceService;
            _uniqueConfidenceService = uniqueConfidenceService;
            _readingLevelConfidenceService = readingLevelConfidenceService;
            _conversationPathService = conversationPathService;
        }

        public List<AnalyzedChat> GetResponseChat(List<MatchChat> conversation, int targetIndex) //TODO: pre-calculation this as part of the naturalLanguageData
        {
            var response = new List<AnalyzedChat>();

            var replyIndex = targetIndex + 1;
            response.Add(conversation[replyIndex].analyzedChat);
            replyIndex++;

            while(replyIndex < conversation.Count)
            {
                if (IsChainedReply(response.Last().chat, conversation[replyIndex].analyzedChat.chat))
                {
                    response.Add(conversation[replyIndex].analyzedChat);
                    replyIndex++;
                }
                else
                {
                    return response;
                }
            }

            return response;
        }
        
        private bool IsChainedReply(Chat chat, Chat reply)
        {
            var maximumReplyTime = 60000;
            return chat.user == reply.user && chat.time + maximumReplyTime > reply.time;
        }

        private List<string> getPathSubjects(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals)
        {
            var subjectStarts = new List<string>();
            if(targetConversation.responses.Count > 0)
            {
                var nld = targetConversation.responses.Last().naturalLanguageData;
                foreach (var subject in nld.subjects)
                {
                    subjectStarts.Add(subject.Lemmas);
                }
            }
            return _conversationPathService.GetPathsSubjects(subjectGoals, subjectStarts, conversationLists);
        }

        public List<ConversationMatchList> GetConversationMatchLists(Conversation targetConversation, List<ConversationList> conversationLists, List<string> subjectGoals)
        {
            var pathSubjects = getPathSubjects(targetConversation, conversationLists, subjectGoals);

            var conversationMatchLists = new List<ConversationMatchList>();

            foreach (var conversationList in conversationLists)
            {
                var conversationMatchList = new ConversationMatchList { type = conversationList.type, matchConversations = new ConcurrentBag<MatchConversation>() };

                Parallel.ForEach(conversationList.conversations, (conversationKeyValuePair) =>
                {
                    var conversation = conversationKeyValuePair.Value;
                    if (conversation.name != targetConversation.name && targetConversation.analyzationVersion != null)
                    {
                        var matchConversation = new MatchConversation
                        {
                            name = conversation.name,
                            responses = new List<MatchChat>()
                        };

                        var subjectMatchConfidence = _subjectConfidenceService.GetSubjectMatchConfidence(targetConversation, conversation);
                        var readingLevelMatchConfidence = _readingLevelConfidenceService.GetReadingLevelConfidence(targetConversation.readingLevel, conversation.readingLevel);

                        for(var index = 0; index < conversation.responses.Count(); index++)
                        {
                            var userlessReply = string.Empty;
                            if(index + 1 < conversation.responses.Count())
                            {
                                userlessReply = conversation.responses[index + 1].naturalLanguageData.userlessMessage;
                            }
                            var matchChat = GetMatch(targetConversation, conversation.responses[index], subjectMatchConfidence, readingLevelMatchConfidence, conversation.groupChat, userlessReply, pathSubjects);
                            matchConversation.responses.Add(matchChat);
                        }

                        conversationMatchList.matchConversations.Add(matchConversation);
                    }
                });

                conversationMatchLists.Add(conversationMatchList);
            }

            return conversationMatchLists;
        }

        private const double replyMatchRatio = .5;
        private const double conversationProximityRatio = .35;
        private const double subjectMatchRatio = .05;
        private const double readingLevelMatchRatio = .05;
        private const double groupChatRatio = .05;

        private MatchChat GetMatch(Conversation targetConversation, AnalyzedChat existingResponse, double subjectMatchConfidence, double readingLevelMatchConfidence, bool existingGroupChat, string userlessReply, List<string> pathSubjects)
        {
            //TODO: add user comparison and user similarity to the algorithm for confidence, if user has same property as bot, etc.
            var targetResponse = targetConversation.responses.Last();

            var matchChat = new MatchChat
            {
                matchConfidence = 0,
                analyzedChat = existingResponse
            };

            if(existingResponse.naturalLanguageData.responseConfidence == 0)
            {
                return matchChat;
            }

            var uniqueConfidence = _uniqueConfidenceService.GetUniqueConfidence(userlessReply, targetConversation);
            var replyConfidence = existingResponse.naturalLanguageData.responseConfidence;
            var confidenceMultiplier = uniqueConfidence * replyConfidence;

            var replyMatchConfidence = _matchConfidenceService.GetMatchConfidence(targetResponse.naturalLanguageData, existingResponse.naturalLanguageData, targetResponse.botName);
            var replyPartScore = replyMatchConfidence * replyMatchRatio;
            //TODO: lower the value of replyMatchConfidence if it's a shorter reply like "yes"

            var conversationProximityScore = _subjectConfidenceService.GetConversationProximityMatchConfidence(targetResponse.naturalLanguageData.proximitySubjects, existingResponse.naturalLanguageData.proximitySubjects);
            var proximityPartScore = conversationProximityScore * conversationProximityRatio;

            var subjectMatchPartScore = subjectMatchConfidence * subjectMatchRatio;

            var readingLevelMatchPartScore = readingLevelMatchConfidence * readingLevelMatchRatio;

            var groupChatMatchConfidence = _groupChatConfidenceService.GetGroupChatConfidence(targetConversation.groupChat, existingGroupChat);
            var groupChatPartScore = groupChatMatchConfidence * groupChatRatio;

            var confidence = (replyPartScore + proximityPartScore + subjectMatchPartScore + readingLevelMatchPartScore + groupChatPartScore) * confidenceMultiplier;

            var length = targetResponse.naturalLanguageData.sentences.SelectMany(s => s.Tokens).Count();
            if(length < 5)
            {
                var exponent = 6 - length;
                confidence = Math.Pow(confidence, exponent);
            }

            var bonusConfidence = getBonusConfidence(existingResponse, pathSubjects);
            confidence = confidence + bonusConfidence;

            matchChat.matchConfidence = confidence;

            return matchChat;
        }

        double goalBonus = .1;
        private double getBonusConfidence(AnalyzedChat existingResponse, List<string> pathSubjects)
        {
            foreach(var conversationSubjects in existingResponse.naturalLanguageData.subjects)
            {
                if(pathSubjects.Contains(conversationSubjects.Lemmas))
                {
                    return goalBonus;
                }
            }
            return 0;
        }
    }
}
