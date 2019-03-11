using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class VoiceService_GetVoice
    {
        [Fact]
        public void IsActive()
        {
            var service = new VoiceService();
            List<Token> tokens = new List<Token>();
            tokens.Add(new Token { IncomingDependencyLabel = "nsubj" });
            var result = service.GetVoice(tokens);
            Assert.Equal(Voice.Active, result);
        }

        [Fact]
        public void IsPassive()
        {
            var service = new VoiceService();
            List<Token> tokens = new List<Token>();
            tokens.Add(new Token { IncomingDependencyLabel = "nsubjpass" });
            var result = service.GetVoice(tokens);
            Assert.Equal(Voice.Passive, result);
        }

        [Fact]
        public void IsUnidentifiable()
        {
            var service = new VoiceService();
            List<Token> tokens = new List<Token>();
            var result = service.GetVoice(tokens);
            Assert.Equal(Voice.Unidentifiable, result);
        }
    }
}
