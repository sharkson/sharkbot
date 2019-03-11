using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class DeclarativeService_IsDeclarative
    {
        [Fact]
        public void ExclamationIsNotDeclarative()
        {
            var service = new DeclarativeService();
            var tokens = new List<Token>();
            tokens.Add(new Token { Word = "!" });
            var result = service.IsDeclarative(tokens);
            Assert.False(result);
        }

        [Fact]
        public void QuestionIsNotDeclarative()
        {
            var service = new DeclarativeService();
            var tokens = new List<Token>();
            tokens.Add(new Token { Word = "?" });
            var result = service.IsDeclarative(tokens);
            Assert.False(result);
        }
    }
}
