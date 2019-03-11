using ChatModels;
using NaturalLanguageService.Services;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SubjectPredicateObjectTokenScoreService_GetScore
    {
        [Fact]
        public void PerfectScore()
        {
            var service = new SubjectPredicateObjectTokenScoreService();

            var target = new Token { Lemmas = "shark", Word = "sharks" };
            var existing = new Token { Lemmas = "Shark", Word = "Sharks" };

            var result = service.GetScore(target, existing);
            Assert.Equal(1, result);
        }

        [Fact]
        public void LemmasMatch()
        {
            var service = new SubjectPredicateObjectTokenScoreService();

            var target = new Token { Lemmas = "shark", Word = "shark" };
            var existing = new Token { Lemmas = "Shark", Word = "Sharks" };

            var result = service.GetScore(target, existing);
            Assert.Equal(.75, result);
        }

        [Fact]
        public void NerMatch()
        {
            var service = new SubjectPredicateObjectTokenScoreService();

            var target = new Token { Lemmas = "Adam", Word = "Adam", NerTag = "Person" };
            var existing = new Token { Lemmas = "Eve", Word = "Eve", NerTag = "Person" };

            var result = service.GetScore(target, existing);
            Assert.Equal(.5, result);
        }

        [Fact]
        public void NoMatch()
        {
            var service = new SubjectPredicateObjectTokenScoreService();

            var target = new Token { Lemmas = "dolphin", Word = "dolphin" };
            var existing = new Token { Lemmas = "Shark", Word = "Sharks" };

            var result = service.GetScore(target, existing);
            Assert.Equal(0, result);
        }

        [Fact]
        public void NoTokenData()
        {
            var service = new SubjectPredicateObjectTokenScoreService();

            var target = new Token { Lemmas = "dolphin", Word = "dolphin", NerTag = "O" };
            var existing = new Token { };
            var result = service.GetScore(target, existing);
            Assert.Equal(0, result);

            target = new Token { };
            existing = new Token { Lemmas = "dolphin", Word = "dolphin", NerTag = "O" };
            result = service.GetScore(target, existing);
            Assert.Equal(0, result);
        }
    }
}
