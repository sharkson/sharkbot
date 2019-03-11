using VaderSharp;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class SentimentAnalyzationService_GetSentiment
    {
        [Fact]
        public void Positive()
        {
            var service = new SentimentAnalyzationService(new SentimentIntensityAnalyzer());
            var result = service.GetSentiment("This sentiment anaylzer works great!");
            Assert.True(result > 0);
        }

        [Fact]
        public void Negative()
        {
            var service = new SentimentAnalyzationService(new SentimentIntensityAnalyzer());
            var result = service.GetSentiment("Stanford sentiment anaylzer is terrible");
            Assert.True(result < 0);
        }

        [Fact]
        public void Neutral()
        {
            var service = new SentimentAnalyzationService(new SentimentIntensityAnalyzer());
            var result = service.GetSentiment("I do sentiment analyzation.");
            Assert.True(result == 0);
        }
    }
}
