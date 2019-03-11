using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ConversationSubjectService_GetProximitySubjects
    {
        [Fact]
        public void GetProximitySubjects()
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
            var conversation = new Conversation();
            conversation.responses = responses;

            var service = new ConversationSubjectService(new ResponseSubjectService());
            var result = service.GetProximitySubjects(conversation, 0);
            Assert.Single(result);
        }

        //TODO: more test coverage for proximity subjects
    }
}
