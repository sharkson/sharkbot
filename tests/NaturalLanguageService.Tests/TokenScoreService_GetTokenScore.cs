using ChatModels;
using NaturalLanguageService.Services;
using System.Collections.Generic;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class TokenScoreService_GetTokenScore
    {
        [Fact]
        public void GetTokenScore()
        {
            var service = new TokenScoreService();
            var targetTokens = new List<Token>();
            targetTokens.Add(new Token { Word = "shark", Lemmas = "shark", PosTag = "NN" });
            var existingTokens = new List<Token>();
            existingTokens.Add(new Token { Word = "sharks", Lemmas = "shark", PosTag = "NN" });
            var result = service.GetScore(targetTokens, existingTokens);

            var targetTokens2 = new List<Token>();
            targetTokens2.Add(new Token { Word = "shark", Lemmas = "shark", PosTag = "NN" });
            var existingTokens2 = new List<Token>();
            existingTokens2.Add(new Token { Word = "whale", Lemmas = "whale", PosTag = "NN" });
            var result2 = service.GetScore(targetTokens2, existingTokens2);
            Assert.True(result > result2);
        }
    }
}
