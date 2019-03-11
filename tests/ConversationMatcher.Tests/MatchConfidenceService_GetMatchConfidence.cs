using ChatAnalyzer.Services;
using ChatModels;
using ConversationMatcher.Services;
using NaturalLanguageService.Services;
using SharkbotConfiguration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class MatchConfidenceService_GetMatchConfidence
    {
        [Fact]
        public void GetMatchConfidence()
        {
            var service = GetService();
            var result = service.GetMatchConfidence(new NaturalLanguageData { sentences = new List<Sentence>() }, new NaturalLanguageData { sentences = new List<Sentence>() }, "sharkbot");
            Assert.Equal(0, result);
        }

        [Fact]
        public void TestRealSentences()
        {
            var analyzationService = GetAnalyzationService();
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

            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "hey",
                    user = "tester2",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);

            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "I like to ride my bike at night.",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);

            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh dude",
                    user = "tester2",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);

            analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "I ate lasagna.",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            conversation.responses.Add(analyzedChat);

            var analyzedConversation = analyzationService.AnalyzeConversationAsync(conversation);
            var target = analyzedConversation.responses[0].naturalLanguageData;
            var existing = analyzedConversation.responses[1].naturalLanguageData;
            var existing2 = analyzedConversation.responses[2].naturalLanguageData;
            var existing3 = analyzedConversation.responses[3].naturalLanguageData;
            var existing4 = analyzedConversation.responses[4].naturalLanguageData;

            var service = GetService();
            var result = service.GetMatchConfidence(target, existing, "sharkbot");
            var result2 = service.GetMatchConfidence(target, existing2, "sharkbot");
            var result3 = service.GetMatchConfidence(target, existing3, "sharkbot");
            var result4 = service.GetMatchConfidence(existing2, existing4, "sharkbot");
        }


        private MatchConfidenceService GetService()
        {
            return new MatchConfidenceService(new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService()));
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
