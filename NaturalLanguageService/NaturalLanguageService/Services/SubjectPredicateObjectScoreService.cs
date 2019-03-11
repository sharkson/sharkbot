using ChatModels;

namespace NaturalLanguageService.Services
{
    public class SubjectPredicateObjectScoreService
    {
        private readonly double lemmaScore = .75;
        private readonly SubjectPredicateObjectTokenScoreService _subjectPredicateObjectTokenScoreService;

        public SubjectPredicateObjectScoreService(SubjectPredicateObjectTokenScoreService subjectPredicateObjectTokenScoreService)
        {
            _subjectPredicateObjectTokenScoreService = subjectPredicateObjectTokenScoreService;
        }

        public double GetScore(Sentence target, Sentence existing)
        {
            var score = 0.0;

            score += _subjectPredicateObjectTokenScoreService.GetScore(target.Subject, existing.Subject);
            score += _subjectPredicateObjectTokenScoreService.GetScore(target.Predicate, existing.Predicate);
            score += _subjectPredicateObjectTokenScoreService.GetScore(target.Object, existing.Object);

            return score / 3;
        }
    }
}
