using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class SentenceTypeService_GetSentenceType
    {
        [Fact]
        public void IsDeclarative()
        {
            var service = new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService());
            var tokens = new List<Token>();
            tokens.Add(new Token { PosTag = "PRP" });
            tokens.Add(new Token { PosTag = "VB" });
            var sentenceType = service.GetSentenceType(tokens);

            Assert.Equal(SentenceType.Declarative, sentenceType);
        }

        [Fact]
        public void IsInterrogative()
        {
            var service = new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService());
            var tokens = new List<Token>();
            tokens.Add(new Token { Word = "?" });
            var sentenceType = service.GetSentenceType(tokens);

            Assert.Equal(SentenceType.Interrogative, sentenceType);
        }

        [Fact]
        public void IsImperative()
        {
            var service = new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService());
            var tokens = new List<Token>();
            tokens.Add(new Token { PosTag = "VBP" });
            var sentenceType = service.GetSentenceType(tokens);

            Assert.Equal(SentenceType.Imperative, sentenceType);
        }

        [Fact]
        public void IsExclamatory()
        {
            var service = new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService());
            var tokens = new List<Token>();
            tokens.Add(new Token { PosTag = "PRP" });
            tokens.Add(new Token { PosTag = "VB" });
            tokens.Add(new Token { Word = "!" });
            var sentenceType = service.GetSentenceType(tokens);

            Assert.Equal(SentenceType.Exclamatory, sentenceType);
        }
    }
}
