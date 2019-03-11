using ChatModels;

namespace NaturalLanguageService.Services
{
    public class SubjectPredicateObjectTokenScoreService
    {
        public double GetScore(Token target, Token existing)
        {
            if(target.Word == null || existing.Word == null)
            {
                return 0;
            }

            if (target.Word.ToLower() == existing.Word.ToLower())
            {
                return 1;
            }
            else if (target.Lemmas.ToLower() == existing.Lemmas.ToLower())
            {
                return .75;
            }
            else if (target.NerTag != null && existing.NerTag != null && target.NerTag.ToLower() == existing.NerTag.ToLower())
            {
                return .5;
            }
            return 0;
        }
    }
}
