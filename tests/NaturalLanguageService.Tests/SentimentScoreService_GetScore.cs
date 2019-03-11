using NaturalLanguageService.Services;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SentimentScoreService_GetScore
    {
        [Fact]
        public void GetScore()
        {
            var service = new SentimentScoreService();

            var result = service.GetScore(0.0, 0.0);
            Assert.Equal(1.0, result);
            result = service.GetScore(1.0, 1.0);
            Assert.Equal(1.0, result);
            result = service.GetScore(-0.5, -0.5);
            Assert.Equal(1.0, result);

            result = service.GetScore(0.0, 1.0);
            Assert.Equal(0.5, result);
            result = service.GetScore(1.0, 0.0);
            Assert.Equal(0.5, result);
            result = service.GetScore(0.25, -0.75);
            Assert.Equal(0.5, result);

            result = service.GetScore(-1.0, 1.0);
            Assert.Equal(0, result);
            result = service.GetScore(1.0, -1.0);
            Assert.Equal(0, result);
        }
    }
}
