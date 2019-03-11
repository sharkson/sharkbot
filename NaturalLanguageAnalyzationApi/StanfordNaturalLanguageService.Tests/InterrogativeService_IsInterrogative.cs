using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class InterrogativeService_IsInterrogative
    {
        [Fact]
        public void IsInterrogative()
        {
            var service = new InterrogativeService();
            var tokens = new List<Token>
            {
                new Token { Word = "?" }
            };
            var result = service.IsInterrogative(tokens);

            Assert.True(result);
        }
    }
}
