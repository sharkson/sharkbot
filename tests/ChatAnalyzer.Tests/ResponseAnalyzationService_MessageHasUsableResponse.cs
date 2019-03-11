using ChatAnalyzer.Services;
using ChatModels;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ResponseAnalyzationService_MessageHasUsableResponse
    {
        [Fact]
        public void MessageHasUsableResponse()
        {
            var service = new ResponseAnalyzationService();
            var message = new Chat
            {
                message = "hello world",
                user = "test"
            };
            var response = new Chat
            {
                message = "suh",
                user = "test2"
            };
            var result = service.MessageHasUsableResponse(message, response);
            Assert.True(result);
        }

        [Fact]
        public void SameUser()
        {
            var service = new ResponseAnalyzationService();
            var message = new Chat
            {
                message = "hello world",
                user = "test"
            };
            var response = new Chat
            {
                message = "suh",
                user = "test"
            };
            var result = service.MessageHasUsableResponse(message, response);
            Assert.False(result);
        }

        [Fact]
        public void Bot()
        {
            var service = new ResponseAnalyzationService();
            var message = new Chat
            {
                message = "hello world",
                user = "test"
            };
            var response = new Chat
            {
                message = "suh",
                user = "testbot",
                botName = "testbot"
            };
            var result = service.MessageHasUsableResponse(message, response);
            Assert.False(result);
        }
    }
}
