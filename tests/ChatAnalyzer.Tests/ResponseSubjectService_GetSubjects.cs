using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ResponseSubjectService_GetSubjects
    {
        [Fact]
        public void GetSubject()
        {
            var sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Lemmas = "ninja" } });
            var analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };

            var service = new ResponseSubjectService();
            var result = service.GetSubjects(analyzedChat);
            Assert.Single(result);
            Assert.Equal("ninja", result[0].Lemmas);
        }

        [Fact]
        public void GetSameSubject()
        {
            var sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Lemmas = "ninja" } });
            sentences.Add(new Sentence { Subject = new Token { Lemmas = "ninja" } });
            var analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };

            var service = new ResponseSubjectService();
            var result = service.GetSubjects(analyzedChat);
            Assert.Single(result);
            Assert.Equal("ninja", result[0].Lemmas);
            Assert.Equal(2, result[0].OccurenceCount);
        }

        [Fact]
        public void GetMultipleSubjects()
        {
            var sentences = new List<Sentence>();
            sentences.Add(new Sentence { Subject = new Token { Lemmas = "ninja" } });
            sentences.Add(new Sentence { Subject = new Token { Lemmas = "people" } });
            var analyzedChat = new AnalyzedChat
            {
                naturalLanguageData = new NaturalLanguageData { sentences = sentences }
            };

            var service = new ResponseSubjectService();
            var result = service.GetSubjects(analyzedChat);
            Assert.Equal(2, result.Count);
        }
    }
}
