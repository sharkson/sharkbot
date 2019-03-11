using ChatModels;
using System.Collections.Generic;

namespace NaturalLanguageService.Services
{
    public class TokenScoreService
    {
        public double GetScore(List<Token> targetTokens, List<Token> existingTokens)
        {
            var score = 0.0;
            foreach (var token in targetTokens)
            {
                score += GetBestTokenScore(token, existingTokens);
            }
            var maxScore = GetMaxScore(targetTokens);
            return score / maxScore;
        }

        private double GetBestTokenScore(Token targetToken, List<Token> tokens)
        {
            var tokenValue = GetTokenValue(targetToken);

            var bestScore = 0.0;

            foreach (var token in tokens)
            {
                var tokenScore = 0.0;

                if (token.Word.ToLower() == targetToken.Word.ToLower())
                {
                    if (token.PosTag == targetToken.PosTag)
                    {
                        tokenScore = tokenValue;
                    }
                    else
                    {
                        tokenScore = .75 * tokenValue;
                    }
                }
                else if(token.Lemmas.ToLower() == targetToken.Lemmas.ToLower())
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

        private double GetMaxScore(List<Token> tokens)
        {
            var score = 0.0;
            foreach (var token in tokens)
            {
                score += GetTokenValue(token);
            }
            return score;
        }

        private double GetTokenValue(Token token)
        {
            if (token.PosTag.StartsWith("JJ"))
            {
                return 0.5;
            }
            if (token.PosTag.StartsWith("NN"))
            {
                return 2;
            }
            if (token.PosTag.StartsWith("PRP"))
            {
                return 0.2;
            }
            if (token.PosTag.StartsWith("RB"))
            {
                return 0.2;
            }
            if (token.PosTag.StartsWith("VB"))
            {
                return 0.2;
            }
            if (token.PosTag.StartsWith("WP"))
            {
                return 0.2;
            }
            return 0.1;
        }
    }
}
