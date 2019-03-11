using ChatModels;
using ConversationMatcher.Services;
using ConversationSteerService;
using ConversationSteerService.Services;
using NaturalLanguageService.Services;
using System.Collections.Generic;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class MatchService_GetResponseChat
    {
        [Fact]
        public void GetResponseChat()
        {
            var service = GetService();
            var conversation = new List<MatchChat>();
            conversation.Add(new MatchChat { analyzedChat = new AnalyzedChat() });
            conversation.Add(new MatchChat { analyzedChat = new AnalyzedChat() });
            var result = service.GetResponseChat(conversation, 0);
            Assert.Single(result);
        }

        public MatchService GetService()
        {
            return new MatchService(new SubjectConfidenceService(), new MatchConfidenceService(new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService())), new GroupChatConfidenceService(), new UniqueConfidenceService(), new ReadingLevelConfidenceService(), new ConversationPathService(new EdgeService(), new VerticeService(), new ShortestPathService()));
        }
    }
}
