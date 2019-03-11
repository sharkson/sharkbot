using ChatModels;
using ConversationMatcher.Services;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class ReadingLevelConfidenceService_GetReadingLevelConfidence
    {
        [Fact]
        public void GetReadingLevelConfidence()
        {
            var service = new ReadingLevelConfidenceService();
            var result = service.GetReadingLevelConfidence(new ReadingLevel(), new ReadingLevel());
            Assert.Equal(1, result);
        }
    }
}
