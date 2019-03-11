using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class PredicateService_GetPredicate
    {
        [Fact]
        public void CorrectSubject()
        {
            var service = new PredicateService();
            var result = service.GetPredicate(new List<OpenieTriple>(), new List<Token>(), SentenceType.Imperative);
            Assert.Null(result.Word);
        }
    }
}
