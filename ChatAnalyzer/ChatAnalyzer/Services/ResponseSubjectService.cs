using ChatModels;
using System.Collections.Generic;

namespace ChatAnalyzer.Services
{
    public class ResponseSubjectService
    {
        public List<ConversationSubject> GetSubjects(AnalyzedChat response)
        {
            var subjects = new List<ConversationSubject>();
            if (response.naturalLanguageData.sentences == null)
            {
                return subjects;
            }

            foreach (var sentence in response.naturalLanguageData.sentences)
            {
                var token = sentence.Subject;
                if (token != null && !string.IsNullOrWhiteSpace(token.Lemmas))
                {
                    var index = subjects.FindIndex(s => s.Lemmas == token.Lemmas);
                    if (index >= 0)
                    {
                        subjects[index].OccurenceCount++;
                    }
                    else
                    {
                        var subject = new ConversationSubject
                        {
                            OccurenceCount = 1,
                            Lemmas = token.Lemmas
                        };
                        subjects.Add(subject);
                    }
                }
            }

            return subjects;
        }
    }
}
