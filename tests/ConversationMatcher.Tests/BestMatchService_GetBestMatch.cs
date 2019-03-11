using ChatModels;
using ConversationMatcher.Services;
using ConversationSteerService;
using ConversationSteerService.Services;
using NaturalLanguageService.Services;
using System.Collections.Generic;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class BestMatchService_GetBestMatch
    {
        [Fact]
        public void GetBestMatch()
        {
            var service = GetService();
            var result = service.GetBestMatch(new Conversation { responses = new List<AnalyzedChat>() }, new List<ConversationList>(), new List<string>());
            Assert.Null(result.analyzedChat);
        }

        public BestMatchService GetService()
        {
            return new BestMatchService(new MatchService(new SubjectConfidenceService(), new MatchConfidenceService(new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService())), new GroupChatConfidenceService(), new UniqueConfidenceService(), new ReadingLevelConfidenceService(), new ConversationPathService(new EdgeService(), new VerticeService(), new ShortestPathService())));
        }
    }
}
