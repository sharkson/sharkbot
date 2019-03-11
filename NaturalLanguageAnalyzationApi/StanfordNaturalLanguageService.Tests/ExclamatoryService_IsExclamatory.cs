using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class ExclamatoryService_IsExclamatory
    {
        [Fact]
        public void IsExclamatory()
        {
            var service = new ExclamatoryService();
            var tokens = new List<Token>
            {
                new Token { PosTag = "PRP" },
                new Token { PosTag = "VB" },
                new Token { Word = "!" }
            };
            var result = service.IsExclamatory(tokens);

            Assert.True(result);
        }
    }
}
