using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class ConversationSubjectService
    {
        private readonly ResponseSubjectService _responseSubjectService;

        public ConversationSubjectService(ResponseSubjectService responseSubjectService)
        {
            _responseSubjectService = responseSubjectService;
        }

        public List<ConversationSubject> GetConversationSubjects(List<AnalyzedChat> responses)
        {
            var subjects = new List<ConversationSubject>();

            foreach (var response in responses)
            {
                foreach(var subject in _responseSubjectService.GetSubjects(response))
                {
                    var subjectIndex = subjects.FindIndex(s => s.Lemmas == subject.Lemmas);
                    if (subjectIndex > -1)
                    {
                        subjects[subjectIndex].OccurenceCount += subject.OccurenceCount;
                    }
                    else
                    {
                        subjects.Add(subject);
                    }
                }
            }

            return subjects;
        }

        public List<ConversationSubject> GetProximitySubjects(Conversation conversation, int index)
        {
            var start = 5;
            if (start < index)
            {
                start = index;
            }

            var take = 10;
            if (conversation.responses.Count < take + index - 1)
            {
                take = conversation.responses.Count - index - 1;
            }

            return GetConversationSubjects(conversation.responses.Skip(index - start).Take(take).ToList());
        }
    }
}
