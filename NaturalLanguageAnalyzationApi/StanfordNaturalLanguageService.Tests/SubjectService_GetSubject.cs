using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class SubjectService_GetSubject
    {
        [Fact]
        public void CorrectSubject()
        {
            var service = new SubjectService();
            var result = service.GetSubject(new List<OpenieTriple>(), new List<Token>(), SentenceType.Imperative);
            Assert.Equal("you", result.Word);
        }

        [Fact]
        public void HandlesNulls()
        {
            var service = new SubjectService();
            var result = service.GetSubject(new List<OpenieTriple>(), new List<Token> { new Token { Word = "you" } }, SentenceType.Imperative);
            Assert.Equal("you", result.Word);
        }
    }
}
