using java.util;
using System.IO;
using System.Linq;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class TokenService_GetTokens
    {
        [Fact]
        public void CorrectTokens()
        {
            Directory.SetCurrentDirectory(@"M:\sharkbot\stanford-corenlp-3.9.1-models");

            var properties = new Properties();
            properties.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref, sentiment");
            properties.setProperty("ner.useSUTime", "0");

            var service = new TokenService();
            var stanfordSentence = new SimpleNetNlp.Sentence("hello world");
            var tokens = service.GetTokens(stanfordSentence);

            Assert.Equal(2, tokens.Count);

            var token = tokens.FirstOrDefault();
            Assert.Equal("hello", token.Word);
            Assert.Equal("hello", token.Lemmas);
            Assert.Equal("O", token.NerTag);
            Assert.Equal("UH", token.PosTag);
        }
    }
}
