using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ResponseAnalyzationService_GetReplyConfidence
    {
        [Fact]
        public void GetReplyConfidence()
        {
            var service = new ResponseAnalyzationService();
            var message = new Chat
            {
                message = "hello world",
                user = "test"
            };
            var analyzedMessage = new AnalyzedChat();
            analyzedMessage.chat = message;

            var response = new Chat
            {
                message = "suh",
                user = "test2"
            };
            var analyzedResponse = new AnalyzedChat();
            analyzedResponse.chat = response;
            var result = service.GetReplyConfidence(analyzedMessage, analyzedResponse, new ConcurrentBag<UserData>(), false);
            Assert.Equal(1, result);
        }

        [Fact]
        public void NotReply()
        {
            
            var tokens = new List<Token>();
            tokens.Add(new Token());
            tokens.Add(new Token());
            tokens.Add(new Token());
            tokens.Add(new Token());
            tokens.Add(new Token());
            var sentence = new Sentence { Tokens = tokens };
            var sentences = new List<Sentence>();
            sentences.Add(sentence);

            var service = new ResponseAnalyzationService();
            var message = new Chat
            {
                message = "hello world",
                user = "test",
                time = 100
            };
            var analyzedMessage = new AnalyzedChat
            {
                chat = message,
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };

            var response = new Chat
            {
                message = "suh",
                user = "test2",
                time = 105
            };
            var analyzedResponse = new AnalyzedChat
            {
                chat = response,
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };
            var result = service.GetReplyConfidence(analyzedMessage, analyzedResponse, new ConcurrentBag<UserData>(), false);
            Assert.Equal(0, result);
        }
    }
}
