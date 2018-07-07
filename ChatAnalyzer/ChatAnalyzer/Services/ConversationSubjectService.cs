using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class ConversationSubjectService
    {
        public List<ConversationSubject> GetConversationSubjects(List<AnalyzedChat> responses)
        {
            var subjects = new List<ConversationSubject>();

            foreach (var response in responses)
            {
                if (!string.IsNullOrWhiteSpace(response.chat.message))
                {
                    subjects = UpdateConversationSubjects(subjects, response);
                }
            }

            return subjects;
        }

        public List<ConversationSubject> UpdateConversationSubjects(List<ConversationSubject> subjects, AnalyzedChat response)
        {
            foreach (var sentence in response.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.POSTag == "NN" || token.POSTag == "NNP")
                    {
                        var lowerCaseToken = token.Lexeme.ToLower();
                        var index = subjects.FindIndex(s => s.subjectWords.Contains(lowerCaseToken));
                        if (index >= 0)
                        {
                            subjects[index].occurenceCount++;
                        }
                        else
                        {
                            var subject = new ConversationSubject();
                            subject.occurenceCount = 1;
                            subject.subjectWords = new List<string>();
                            subject.subjectWords.Add(lowerCaseToken);
                            subjects.Add(subject);
                        }
                    }
                    else if (token.POSTag == "NNS" || token.POSTag == "NNPS")
                    {
                        var lowerCaseToken = token.Lexeme.ToLower();
                        var singularForms = NaturalLanguageService.NaturalLanguageService.GetSingularForms(lowerCaseToken);
                        var index = subjects.FindIndex(s => s.subjectWords.Where(sw => singularForms.Contains(sw)).Any());
                        if (index >= 0)
                        {
                            subjects[index].occurenceCount++;
                        }
                        else
                        {
                            var subject = new ConversationSubject();
                            subject.occurenceCount = 1;
                            subject.subjectWords = new List<string>();
                            subject.subjectWords.AddRange(singularForms);
                            subjects.Add(subject);
                        }
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
