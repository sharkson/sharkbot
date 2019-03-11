using System;
using ChatModels;

namespace NaturalLanguageService.Services
{
    public class SentenceTypeScoreService
    {
        public double GetScore(SentenceType target, SentenceType existing)
        {
            if(target == existing)
            {
                return 1.0;
            }

            if (target == SentenceType.Unidentifiable || existing == SentenceType.Unidentifiable)
            {
                return 0.25;
            }

            if (target == SentenceType.Interrogative)
            {
                return 0.0;
            }

            if (target == SentenceType.Imperative)
            {
                return 0.0;
            }

            if (target == SentenceType.Declarative && existing == SentenceType.Exclamatory)
            {
                return .75;
            }
            if (target == SentenceType.Exclamatory && existing == SentenceType.Declarative)
            {
                return .75;
            }

            return 0.0;
        }
    }
}
