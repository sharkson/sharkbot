using ChatModels;

namespace ConversationMatcher.Services
{
    public class ScoreService
    {
        public double getTotalPossibleScore(NaturalLanguageData target, string botName, double tripletMatchValue)
        {
            //TODO: exclude names, or make them less weight
            var score = 0.0;
            foreach (var sentence in target.sentences)
            {
                if (sentence.triplets.subject != null)
                {
                    score += tripletMatchValue;
                }
                else
                {
                    foreach (var token in sentence.tokens)
                    {
                        var tagValue = NaturalLanguageService.NaturalLanguageService.POSTagValues[token.POSTag];
                        if (tagValue != null && token.Lexeme.ToLower() != botName.ToLower() && token.Lexeme.ToLower() != "@" + botName.ToLower())
                        {
                            score += (double)tagValue + ((double)tagValue * .1);
                        }
                    }
                }
            }
            return score;
        }
    }
}
