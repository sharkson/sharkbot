using ChatModels;
using ConversationMatcher.Services;
using System.Collections.Generic;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class SubjectConfidenceService_GetConversationProximityMatchConfidence
    {
        [Fact]
        public void GetSubjectMatchConfidence()
        {
            var service = new SubjectConfidenceService();
            var result = service.GetConversationProximityMatchConfidence(new List<ConversationSubject>(), new List<ConversationSubject>());
            Assert.Equal(1, result);

            var target = new List<ConversationSubject>();
            target.Add(new ConversationSubject { Lemmas = "foo", OccurenceCount = 1 });
            var existing = new List<ConversationSubject>();
            existing.Add(new ConversationSubject { Lemmas = "bar", OccurenceCount = 1 });
            result = service.GetConversationProximityMatchConfidence(target, existing);
            Assert.Equal(0, result);

            target = new List<ConversationSubject>();
            target.Add(new ConversationSubject { Lemmas = "foo", OccurenceCount = 1 });
            target.Add(new ConversationSubject { Lemmas = "bar", OccurenceCount = 1 });
            existing = new List<ConversationSubject>();
            existing.Add(new ConversationSubject { Lemmas = "bar", OccurenceCount = 1 });
            existing.Add(new ConversationSubject { Lemmas = "nothing", OccurenceCount = 1 });
            result = service.GetConversationProximityMatchConfidence(target, existing);
            Assert.Equal(.5, result);
        }
    }
}
