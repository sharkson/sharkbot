using ChatModels;
using SharkbotReplier.Services;
using System.Collections.Generic;
using Xunit;

namespace SharkbotReplier.Tests
{
    public class UrbanDictionaryMatchService_GetUrbanDictionaryMatch
    {
        [Fact]
        public void GetDefinition()
        {
            var service = new UrbanDictionaryMatchService();
            var conversation = new Conversation { responses = new List<AnalyzedChat>() };
            conversation.responses.Add(new AnalyzedChat { chat = new Chat { message = "what is love?" }, botName = "sharkbot", naturalLanguageData = new NaturalLanguageData { sentences = new List<Sentence> { new Sentence { Subject = new Token { Lemmas = "love" } } } }  });
            var result = service.GetUrbanDictionaryMatch(conversation);

            Assert.Single(result.response);
        }
    }
}
