using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ConversationMatcher.Services
{
    public class SubjectConfidenceService
    {
        public double GetSubjectMatchConfidence(Conversation targetConversation, Conversation existingConversation)
        {
            //TODO: exclude names, or make them less weight, maybe use natural language name identifier
            var score = 0.0;
            foreach (var targetSubjects in new List<ConversationSubject>(targetConversation.subjects))
            {
                foreach (var subjects in existingConversation.subjects)
                {
                    if (subjects.Lemmas == targetSubjects.Lemmas)
                    {
                        score += targetSubjects.OccurenceCount;
                        break;
                    }
                }
            }

            var maxScore = targetConversation.subjects.Sum(s => s.OccurenceCount);
            if (maxScore == 0)
            {
                return 1;
            }

            return score / maxScore;
        }

        public double GetConversationProximityMatchConfidence(List<ConversationSubject> targetSubjectList, List<ConversationSubject> existingSubjectList)
        {
            var score = 0.0;
            foreach (var targetSubjects in targetSubjectList)
            {
                foreach (var subjects in existingSubjectList)
                {
                    if (subjects.Lemmas == targetSubjects.Lemmas)
                    {
                        score += targetSubjects.OccurenceCount;
                        break;
                    }
                }
            }

            var maxScore = targetSubjectList.Sum(s => s.OccurenceCount);
            if (maxScore == 0)
            {
                return 1;
            }

            return score / maxScore;
        }
    }
}
