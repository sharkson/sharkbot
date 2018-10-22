using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class ResponseSubjectService
    {
        public List<ConversationSubject> GetSubjects(AnalyzedChat response)
        {
            var subjects = new List<ConversationSubject>();

            foreach (var sentence in response.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.POSTag == "NN" || token.POSTag == "NNP" || token.POSTag == "NNS" || token.POSTag == "NNPS")
                    {
                        var index = subjects.FindIndex(s => s.subjectWords.Contains(token.Stem));
                        if (index >= 0)
                        {
                            subjects[index].occurenceCount++;
                        }
                        else
                        {
                            var subject = new ConversationSubject();
                            subject.occurenceCount = 1;
                            subject.subjectWords = new List<string>();
                            subject.subjectWords.Add(token.Stem);
                            subjects.Add(subject);
                        }
                    }
                }
            }

            return subjects;
        }
    }
}
