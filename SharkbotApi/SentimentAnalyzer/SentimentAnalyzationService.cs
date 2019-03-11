using VaderSharp;

namespace SentimentAnalyzer
{
    public class SentimentAnalyzationService
    {
        private readonly SentimentIntensityAnalyzer _sentimentIntensityAnalyzer;

        public SentimentAnalyzationService(SentimentIntensityAnalyzer sentimentIntensityAnalyzer)
        {
            _sentimentIntensityAnalyzer = sentimentIntensityAnalyzer;
        }

        public double GetSentiment(string text)
        {
            var results = _sentimentIntensityAnalyzer.PolarityScores(text);

            return results.Compound;
        }
    }
}
