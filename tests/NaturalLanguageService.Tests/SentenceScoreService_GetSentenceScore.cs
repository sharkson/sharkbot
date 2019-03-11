using ChatAnalyzer.Services;
using ChatModels;
using NaturalLanguageService.Services;
using SharkbotConfiguration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SentenceScoreService_GetSentenceScore
    {
        [Fact]
        public void GetSentenceScore()
        {
            var service = new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService());
            var targetTriples = new List<OpenieTriple>();
            targetTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var target = new Sentence { OpenieTriples = targetTriples, Subject = new Token { Word = "sharks" }, Object = new Token { Word = "sharks" }, Predicate = new Token { Word = "sharks" } };

            var result = service.GetScore(target, target);
            Assert.Equal(1, result);
        }

        [Fact]
        public void TestRealSentences()
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

            var analyzedConversation = service.AnalyzeConversationAsync(conversation);
            var target = analyzedConversation.responses[0].naturalLanguageData.sentences[0];
            var existing = analyzedConversation.responses[1].naturalLanguageData.sentences[0];
            var existing2 = analyzedConversation.responses[2].naturalLanguageData.sentences[0];
            var existing3 = analyzedConversation.responses[3].naturalLanguageData.sentences[0];

            var scoreService = new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService());
            var result = scoreService.GetScore(target, existing);
            var result2 = scoreService.GetScore(target, existing2);
            var result3 = scoreService.GetScore(target, existing3);
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
