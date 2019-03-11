using ChatModels;
using NaturalLanguageService.Services;

namespace ConversationMatcher.Services
{
    public class MatchConfidenceService
    {
        private readonly SentenceScoreService _sentenceScoreService;

        public MatchConfidenceService(SentenceScoreService sentenceScoreService)
        {
            _sentenceScoreService = sentenceScoreService;
        }

        public double GetMatchConfidence(NaturalLanguageData target, NaturalLanguageData existing, string botName)
        {
            var confidence = GetNaturalLanguageDataScore(target, existing);
            var reverseConfidence = GetNaturalLanguageDataScore(existing, target);
            var score = (confidence + reverseConfidence) / 2.0;

            return score;
        }

        private double GetNaturalLanguageDataScore(NaturalLanguageData target, NaturalLanguageData existing)
        {
            var sentencesScore = 0.0;
            foreach (var targetSentence in target.sentences)
            {
                var bestScore = 0.0;
                foreach(var existingSentence in existing.sentences)
                {
                    var score = _sentenceScoreService.GetScore(targetSentence, existingSentence);
                    if(score > bestScore)
                    {
                        bestScore = score;
                    }
                }
                sentencesScore += bestScore;
            }

            var maxSentencesScore = (double)target.sentences.Count;
            if (maxSentencesScore == 0)
            {
                return 0;
            }
            return sentencesScore / maxSentencesScore;
        }
    }
}
