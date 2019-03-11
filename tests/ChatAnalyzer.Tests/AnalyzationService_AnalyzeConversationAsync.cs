using ChatAnalyzer.Services;
using ChatModels;
using NaturalLanguageService.Services;
using SharkbotConfiguration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class AnalyzationService_AnalyzeConversationAsync
    {
        [Fact]
        public void AnalyzeConversationAsync()
        {
            var service = GetAnalyzationService();
            var conversation = new Conversation();
            conversation.name = "Test";
            conversation.responses = new List<AnalyzedChat>();
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
            conversation.responses.Add(analyzedChat);
            var result = service.AnalyzeConversationAsync(conversation);
            Assert.Equal(conversation.name, result.name);
            Assert.False(result.groupChat);
            Assert.Single(result.responses);
        }

        [Fact]
        public void AnalyzeSpecialCharactersConversationAsync()
        {
            var service = GetAnalyzationService();
            var conversation = new Conversation();
            conversation.name = "Test";
            conversation.responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "尸尺乇尸卂尺乇 下口尺 丅尺口凵乃乚乇 卂𠘨刀 从卂长乇 工丅 刀口凵乃乚乇!",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);
            var result = service.AnalyzeConversationAsync(conversation);
            Assert.Equal(conversation.name, result.name);
            Assert.False(result.groupChat);
            Assert.Single(result.responses);
        }

        [Fact]
        public void AnalyzeWeirdCharactersConversationAsync()
        {
            var service = GetAnalyzationService();
            var conversation = new Conversation();
            conversation.name = "Test";
            conversation.responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "⣿⠄⡇⢸⣟⠄⠁⢸⡽⠖⠛⠈⡉⣉⠉⠋⣁⢘⠉⢉⠛⡿⢿⣿⣿⣿⣿⣿⣿⣿ ⣷⣶⣷⣤⠄⣠⠖⠁⠄⠂⠁⠄⠄⠉⠄⠄⠎⠄⠠⠎⢐⠄⢑⣛⠻⣿⣿⣿⣿⣿ ⣿⣿⣿⠓⠨⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠈⠐⠅⠄⠉⠄⠗⠆⣸⣿⣿⣿⣿⣿ ⣿⣿⣿⡣⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⢰⣤⣦⠄⠄⠄⠄⠄⠄⠄⡀⡙⣿⣿⣿⣿ ⣿⣿⡛⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠔⠿⡿⠿⠒⠄⠠⢤⡀⡀⠄⠁⠄⢻⣿⣿⣿ ⣿⣿⠄⠄⠄⠄⠄⠄⣠⡖⠄⠁⠁⠄⠄⠄⠄⠄⠄⠄⣽⠟⡖⠄⠄⠄⣼⣿⣿⣿ ⣿⣿⠄⠄⠄⠄⠄⠄⢠⣠⣀⠄⠄⠄⠄⢀⣾⣧⠄⠂⠸⣈⡏⠄⠄⠄⣿⣿⣿⣿ ⣿⣿⡞⠄⠄⠄⠄⠄⢸⣿⣶⣶⣶⣶⣶⡿⢻⡿⣻⣶⣿⣿⡇⠄⠄⠄⣿⣿⣿⣿ ⣿⣿⡷⡂⠄⠄⠁⠄⠸⣿⣿⣿⣿⣿⠟⠛⠉⠉⠙⠛⢿⣿⡇⠄⠄⢀⣿⣿⣿⣿ ⣶⣶⠃⠄⠄⠄⠄⠄⠄⣾⣿⣿⡿⠁⣀⣀⣤⣤⣤⣄⢈⣿⡇⠄⠄⢸⣿⣿⣿⣿ ⣿⣯⠄⠄⠄⠄⠄⠄⠄⢻⣿⣿⣷⣶⣿⣿⣥⣬⣿⣿⣟⣿⠃⠄⠨⠺⢿⣿⣿⣿ ⠱⠂⠄⠄⠄⠄⠄⠄⠄⣬⣸⡝⠿⢿⣿⡿⣿⠻⠟⠻⢫⡁⠄⠄⠄⡐⣾⣿⣿⣿ ⡜⠄⠄⠄⠄⠄⠆⡐⡇⢿⣽⣻⣷⣦⣧⡀⡀⠄⠄⣴⣺⡇⠄⠁⠄⢣⣿⣿⣿⣿ ⠡⠱⠄⠄⠡⠄⢠⣷⠆⢸⣿⣿⣿⣿⣿⣿⣷⣿⣾⣿⣿⡇⠄⠄⠠⠁⠿⣿⣿⣿ ⢀⣲⣧⣷⣿⢂⣄⡉⠄⠘⠿⣿⣿⣿⡟⣻⣯⠿⠟⠋⠉⢰⢦⠄⠊⢾⣷⣮⣽⣛",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);
            var result = service.AnalyzeConversationAsync(conversation);
            Assert.Equal(conversation.name, result.name);
            Assert.False(result.groupChat);
            Assert.Single(result.responses);
        }

        [Fact]
        public void AnalyzeEmojisConversationAsync()
        {
            var service = GetAnalyzationService();
            var conversation = new Conversation();
            conversation.name = "Test";
            conversation.responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "I'm a 37.5% 🤔 ",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);
            var result = service.AnalyzeConversationAsync(conversation);
            Assert.Equal(conversation.name, result.name);
            Assert.False(result.groupChat);
            Assert.Single(result.responses);
        }


        public AnalyzationService GetAnalyzationService()
        {
            UserDatabase.UserDatabase.userDatabase = new ConcurrentBag<UserData>();
            ConfigurationService.AnalyzationVersion = ".5";
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50214/");
            var service = new NaturalLanguageApiService(client);
            return new AnalyzationService(new ConversationSubjectService(new ResponseSubjectService()), new ResponseAnalyzationService(), new ConversationTypeService(), new UserlessMessageService(), new ConversationReadingLevelService(), new ResponseSubjectService(), service);
        }
    }
}
