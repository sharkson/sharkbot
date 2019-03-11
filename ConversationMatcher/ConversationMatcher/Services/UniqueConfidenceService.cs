using ChatModels;
using System.Linq;

namespace ConversationMatcher.Services
{
    public class UniqueConfidenceService
    {
        public double GetUniqueConfidence(string responseWithoutUsers, Conversation conversation)
        {
            var maxConfidence = 1.0;

            if (conversation.responses.Any(r => r.naturalLanguageData != null && r.naturalLanguageData.userlessMessage == responseWithoutUsers))
            {
                if (conversation.responses.Count() < 10 || conversation.responses.Skip(conversation.responses.Count - 10).Take(10).Any(r => r.naturalLanguageData != null && r.naturalLanguageData.userlessMessage == responseWithoutUsers))
                {
                    return maxConfidence * .1;
                }
                else if (conversation.responses.Count() < 25 || conversation.responses.Skip(conversation.responses.Count - 25).Take(25).Any(r => r.naturalLanguageData != null && r.naturalLanguageData.userlessMessage == responseWithoutUsers))
                {
                    return maxConfidence * .5;
                }
                return maxConfidence * .75;
            }

            return maxConfidence;
        }
    }
}
