using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class ImperativeService_IsImperative
    {
        [Fact]
        public void IsImperative()
        {
            var service = new ImperativeService();
            var tokens = new List<Token>
            {
                new Token { PosTag = "VBP" }
            };
            var result = service.IsImperative(tokens);

            Assert.True(result);
        }
    }
}
