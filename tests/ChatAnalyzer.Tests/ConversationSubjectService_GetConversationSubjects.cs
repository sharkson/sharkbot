using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ConversationSubjectService_GetConversationSubjects
    {
        [Fact]
        public void GetConversationSubject()
        {
            var responses = new List<AnalyzedChat>();
            var sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Word = "ninjas", Lemmas = "ninja" } });
            var analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };
            responses.Add(analyzedChat);
            analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };
            responses.Add(analyzedChat);

            var service = new ConversationSubjectService(new ResponseSubjectService());
            var result = service.GetConversationSubjects(responses);
            Assert.Single(result);
            Assert.Equal(2, result[0].OccurenceCount);
        }

        [Fact]
        public void GetConversationSubjects()
        {
            var responses = new List<AnalyzedChat>();
            var sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Word = "ninjas", Lemmas = "ninja" } });
            var analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };
            responses.Add(analyzedChat);

            sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Word = "ponies", Lemmas = "pony" } });
            analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };
            responses.Add(analyzedChat);

            var service = new ConversationSubjectService(new ResponseSubjectService());
            var result = service.GetConversationSubjects(responses);
            Assert.Equal(2, result.Count);
        }
    }
}
