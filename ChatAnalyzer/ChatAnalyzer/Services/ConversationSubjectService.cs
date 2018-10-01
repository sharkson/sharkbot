using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class ConversationSubjectService
    {
        public ResponseSubjectService responseSubjectService;

        public ConversationSubjectService()
        {
            responseSubjectService = new ResponseSubjectService();
        }

        public List<ConversationSubject> GetConversationSubjects(List<AnalyzedChat> responses)
        {
            var subjects = new List<ConversationSubject>();

            foreach (var response in responses)
            {
                if (!string.IsNullOrWhiteSpace(response.chat.message))
                {
                    subjects.AddRange(responseSubjectService.GetSubjects(response));
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
