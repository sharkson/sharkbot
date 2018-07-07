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
                else if ((token.POSTag == "NNS" && targetToken.POSTag == "NN") || (token.POSTag == "NNPS" && targetToken.POSTag == "NNP"))
                {
                    if (NaturalLanguageService.NaturalLanguageService.GetSingularForms(token.Lexeme.ToLower()).Contains(targetToken.Lexeme.ToLower()))
                    {
                        tokenScore = .75 * tokenValue;
                    }
                }
                else if ((token.POSTag == "NN" && targetToken.POSTag == "NNS") || (token.POSTag == "NNP" && targetToken.POSTag == "NNPS"))
                {
                    if (NaturalLanguageService.NaturalLanguageService.GetSingularForms(targetToken.Lexeme.ToLower()).Contains(token.Lexeme.ToLower()))
                    {
                        tokenScore = .75 * tokenValue;
                    }
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
