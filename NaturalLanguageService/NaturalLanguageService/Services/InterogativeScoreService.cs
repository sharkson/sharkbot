using ChatModels;
using System.Linq;

namespace ConversationMatcher.Services
{
    public class InterogativeScoreService
    {
        public double getInterrogativeScore(NaturalLanguageData target, NaturalLanguageData naturalLanguageDocument)
        {
            if (target.sentences.Any(s => s.interrogative) != naturalLanguageDocument.sentences.Any(s => s.interrogative))
            {
                return .5;
            }
            return 1;
        }
    }
}
