using ChatModels;
using NaturalLanguageService.Services;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SubjectPredicateObjectScoreService_GetScore
    {
        [Fact]
        public void PerfectScore()
        {
            var service = new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService());

            var target = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };
            var existing = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };

            var result = service.GetScore(target, existing);
            Assert.Equal(1, result);
        }

        [Fact]
        public void TwoMatch()
        {
            var service = new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService());

            var target = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };
            var existing = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "pizza", Word = "pizza" }, };

            var result = service.GetScore(target, existing);
            Assert.Equal(2.0 / 3, result);
        }

        [Fact]
        public void OneMatch()
        {
            var service = new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService());

            var target = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };
            var existing = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "make", Word = "make" }, Object = new Token { Lemmas = "pizza", Word = "pizza" }, };

            var result = service.GetScore(target, existing);
            Assert.Equal(1.0 / 3, result);
        }

        [Fact]
        public void NoMatch()
        {
            var service = new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService());

            var target = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };
            var existing = new Sentence { Subject = new Token { Lemmas = "dolphin", Word = "dolphins" }, Predicate = new Token { Lemmas = "make", Word = "make" }, Object = new Token { Lemmas = "pizza", Word = "pizza" }, };

            var result = service.GetScore(target, existing);
            Assert.Equal(0, result);
        }

        [Fact]
        public void OneLemmasMatch()
        {
            var service = new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService());

            var target = new Sentence { Subject = new Token { Lemmas = "shark", Word = "sharks" }, Predicate = new Token { Lemmas = "eat", Word = "eat" }, Object = new Token { Lemmas = "meat", Word = "meat" }, };
            var existing = new Sentence { Subject = new Token { Lemmas = "shark", Word = "shark" }, Predicate = new Token { Lemmas = "make", Word = "make" }, Object = new Token { Lemmas = "pizza", Word = "pizza" }, };

            var result = service.GetScore(target, existing);
            Assert.Equal(.75 / 3, result);
        }
    }
}
