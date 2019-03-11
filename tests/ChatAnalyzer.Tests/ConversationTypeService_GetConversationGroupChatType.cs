using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ConversationTypeService_GetConversationGroupChatType
    {
        [Fact]
        public void IsGroupChat()
        {
            var responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);
            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester2",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);
            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester3",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);

            var service = new ConversationTypeService();
            var result = service.GetConversationGroupChatType(responses);
            Assert.True(result);
        }

        [Fact]
        public void IsNotGroupChat()
        {
            var responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);
            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester2",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);
            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);

            var service = new ConversationTypeService();
            var result = service.GetConversationGroupChatType(responses);
            Assert.False(result);
        }
    }
}
