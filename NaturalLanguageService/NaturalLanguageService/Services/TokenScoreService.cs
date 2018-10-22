using ChatModels;
using System.Collections.Generic;

namespace ConversationMatcher.Services
{
    public class TokenScoreService
    {
        public double getTokenValue(Token token)
        {
            var tagScore = NaturalLanguageService.NaturalLanguageService.POSTagValues[token.POSTag];
            if(tagScore != null)
            {
                return tagScore;
            }
            return 0;
        }

        public double getTokenScore(Token targetToken, List<Token> tokens)
        {
            var tokenValue = getTokenValue(targetToken);

            var bestScore = 0.0;

            foreach (var token in tokens)
            {
                var tokenScore = 0.0;

                if (token.Lexeme.ToLower() == targetToken.Lexeme.ToLower())
                {
                    if (token.POSTag == targetToken.POSTag)
                    {
                        tokenScore = tokenValue;
                    }
                    else
                    {
                        tokenScore = .75 * tokenValue;
                    }
                }
                else if(token.Stem == targetToken.Stem)
                {
                    tokenScore = .75 * tokenValue;
                }

                if (tokenScore > bestScore)
                {
                    bestScore = tokenScore;
                }
            }

            return bestScore;
        }

        public double getTokenScore(Token targetToken, List<Sentence> sentences)
        {
            var tokens = new List<Token>();

            foreach (var sentence in sentences)
            {
                tokens.AddRange(sentence.tokens);
            }

            return getTokenScore(targetToken, tokens);
        }
    }
}
