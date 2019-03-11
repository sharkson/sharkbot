using ChatModels;
using ConversationMatcher.Services;
using System.Collections.Generic;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class UniqueConfidenceService_GetUniqueConfidence
    {
        [Fact]
        public void GetUniqueConfidence()
        {
            var service = new UniqueConfidenceService();
            var result = service.GetUniqueConfidence("suh", new Conversation { responses = new List<AnalyzedChat>() });
            Assert.Equal(1, result);

            var responses = new List<AnalyzedChat>();
            responses.Add(new AnalyzedChat { naturalLanguageData = new NaturalLanguageData { userlessMessage = "suh" } });
            result = service.GetUniqueConfidence("suh", new Conversation { responses = responses });
            Assert.Equal(0.1, result);
        }
    }
}
