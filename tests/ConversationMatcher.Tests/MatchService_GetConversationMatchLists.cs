using ChatModels;
using ConversationMatcher.Services;
using ConversationSteerService;
using ConversationSteerService.Services;
using NaturalLanguageService.Services;
using System.Collections.Generic;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class MatchService_GetConversationMatchLists
    {
        [Fact]
        public void GetConversationMatchLists()
        {
            var service = GetService();
            var result = service.GetConversationMatchLists(new Conversation { responses = new List<AnalyzedChat>() }, new List<ConversationList>(), new List<string>());
            Assert.Empty(result);
        }

        public MatchService GetService()
        {
            return new MatchService(new SubjectConfidenceService(), new MatchConfidenceService(new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService())), new GroupChatConfidenceService(), new UniqueConfidenceService(), new ReadingLevelConfidenceService(), new ConversationPathService(new EdgeService(), new VerticeService(), new ShortestPathService()));
        }
    }
}
