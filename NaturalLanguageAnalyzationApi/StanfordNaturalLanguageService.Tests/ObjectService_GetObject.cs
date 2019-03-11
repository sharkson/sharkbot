using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class ObjectService_GetObject
    {
        [Fact]
        public void CorrectSubject()
        {
            var service = new ObjectService();
            var result = service.GetObject(new List<OpenieTriple>(), new List<Token>(), SentenceType.Imperative);
            Assert.Null(result.Word);
        }
    }
}
