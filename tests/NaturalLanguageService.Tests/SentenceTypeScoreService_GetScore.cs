using ChatModels;
using NaturalLanguageService.Services;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SentenceTypeScoreService_GetScore
    {
        [Fact]
        public void GetScore()
        {
            var service = new SentenceTypeScoreService();
            var result = service.GetScore(SentenceType.Declarative, SentenceType.Declarative);
            Assert.Equal(1.0, result);

            result = service.GetScore(SentenceType.Exclamatory, SentenceType.Declarative);
            Assert.Equal(0.75, result);

            result = service.GetScore(SentenceType.Imperative, SentenceType.Declarative);
            Assert.Equal(0.0, result);

            result = service.GetScore(SentenceType.Interrogative, SentenceType.Declarative);
            Assert.Equal(0.0, result);

            result = service.GetScore(SentenceType.Unidentifiable, SentenceType.Declarative);
            Assert.Equal(0.25, result);
        }
    }
}
