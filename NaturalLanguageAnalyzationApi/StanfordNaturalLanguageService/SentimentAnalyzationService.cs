using VaderSharp;

namespace StanfordNaturalLanguageService
{
    public class SentimentAnalyzationService
    {
        private readonly SentimentIntensityAnalyzer _sentimentIntensityAnalyzer;

        public SentimentAnalyzationService(SentimentIntensityAnalyzer sentimentIntensityAnalyzer)
        {
            _sentimentIntensityAnalyzer = sentimentIntensityAnalyzer;
        }

        /// <summary>
        /// This works much better than the Stanford NLP sentiment analyzer
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public double GetSentiment(string text)
        {
            var results = _sentimentIntensityAnalyzer.PolarityScores(text);

            return results.Compound;
        }
    }
}
