using ChatModels;

namespace ConversationMatcher.Services
{
    public class MatchConfidenceService
    {
        private TripletScoreService tripletScoreService;
        private TokenScoreService tokenScoreService;
        private InterogativeScoreService interogativeScoreService;
        private ScoreService scoreService;

        public MatchConfidenceService()
        {
            tripletScoreService = new TripletScoreService();
            tokenScoreService = new TokenScoreService();
            interogativeScoreService = new InterogativeScoreService();
            scoreService = new ScoreService();
        }

        public double getMatchConfidence(NaturalLanguageData target, NaturalLanguageData naturalLanguageDocument, string botName)
        {
            var confidence = getOneWayMatchConfidence(target, naturalLanguageDocument, botName);
            var reverseConfidence = getOneWayMatchConfidence(naturalLanguageDocument, target, botName);
            var score = (confidence + reverseConfidence) / 2.0;

            return score;
        }

        public double getOneWayMatchConfidence(NaturalLanguageData target, NaturalLanguageData naturalLanguageDocument, string botName)
        {
            //TODO: exclude names, match them, or make them less weight
            var sentencesScore = 0.0;
            foreach (var targetSentence in target.sentences)
            {
                sentencesScore += getSentenceScore(targetSentence, naturalLanguageDocument, botName);
            }
            var maxSentencesScore = target.sentences.Count;
            var score = sentencesScore / maxSentencesScore;

            score = score * interogativeScoreService.getInterrogativeScore(target, naturalLanguageDocument);

            return score;
        }

        public double getSentenceScore(Sentence targetSentence, NaturalLanguageData naturalLanguageDocument, string botName)
        {           
            if (targetSentence.triplets.subject != null)
            {
                return tripletScoreService.getBestTripletScore(targetSentence.triplets, naturalLanguageDocument.sentences);
            }
            else
            {
                var score = 0.0;
                var maximumScore = 0.0;
                foreach (var targetToken in targetSentence.tokens)
                {
                    score += tokenScoreService.getTokenScore(targetToken, naturalLanguageDocument.sentences);
                    maximumScore += tokenScoreService.getTokenValue(targetToken);
                }
                return score / maximumScore;
            }
        }
    }
}
