using ChatModels;

namespace NaturalLanguageService.Services
{
    public class SentenceScoreService
    {
        private readonly OpenieScoreService _openieScoreService;
        private readonly SubjectPredicateObjectScoreService _subjectPredicateObjectScoreService;
        private readonly TokenScoreService _tokenScoreService;
        private readonly SentimentScoreService _sentimentScoreService;
        private readonly SentenceTypeScoreService _sentenceTypeScoreService;
        private readonly VoiceScoreService _voiceScoreService;

        public SentenceScoreService(OpenieScoreService openieScoreService, SubjectPredicateObjectScoreService subjectPredicateObjectScoreService, TokenScoreService tokenScoreService, SentimentScoreService sentimentScoreService, SentenceTypeScoreService sentenceTypeScoreService, VoiceScoreService voiceScoreService)
        {
            _openieScoreService = openieScoreService;
            _subjectPredicateObjectScoreService = subjectPredicateObjectScoreService;
            _tokenScoreService = tokenScoreService;
            _sentimentScoreService = sentimentScoreService;
            _sentenceTypeScoreService = sentenceTypeScoreService;
            _voiceScoreService = voiceScoreService;
        }

        public double GetScore(Sentence target, Sentence existing)
        {
            var maxScore = 0.0;
            var score = 0.0;

            var value = 1.0;
            if(target.OpenieTriples.Count > 0)
            {
                value = 6.0;
                maxScore += value;
                var openieScore = _openieScoreService.GetOpenieScore(target.OpenieTriples, existing.OpenieTriples);
                score += (openieScore * value);
            }

            if (target.Subject.Word != null)
            {
                value = 4.0;
                maxScore += value;
                var spoScore = _subjectPredicateObjectScoreService.GetScore(target, existing);
                score += (spoScore * value);
            }
            else
            {             
                value = 1.0;
                maxScore += value;
                var tokenScore = _tokenScoreService.GetScore(target.Tokens, existing.Tokens);
                score += (tokenScore * value);
            }

            var sentimentScore = _sentimentScoreService.GetScore(target.Sentiment, existing.Sentiment);
            score *= sentimentScore;

            if (target.SentenceType != SentenceType.Unidentifiable)
            {
                var sentenceTypeScore = _sentenceTypeScoreService.GetScore(target.SentenceType, existing.SentenceType);
                score *= sentimentScore;
            }

            if (target.Voice != Voice.Unidentifiable)
            {
                var voiceScore = _voiceScoreService.GetScore(target.Voice, existing.Voice);
                score *= voiceScore;
            }

            return score / maxScore;
        }
    }
}
