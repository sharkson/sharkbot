using ChatModels;
using System.Collections.Concurrent;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class ResponseAnalyzationService
    {
        public bool MessageHasUsableResponse(Chat message, Chat response)
        {
            if (response == null)
            {
                return false;
            }

            if (response.user == response.botName)
            {
                return false;
            }

            if (message.user == response.user)
            {
                return false;
            }

            return true;
        }

        private double wordsReadPerSecond = 6;
        private double wordsTypedPerSecond = 2.5;
        private double maximumResponseTime = 60;
        private double minimumResponseTime = 1;

        public double GetReplyConfidence(AnalyzedChat chat, AnalyzedChat response, ConcurrentBag<UserData> users, bool groupChat)
        {
            var maxConfidence = 1.0;

            if (groupChat)
            {
                if (!response.chat.message.ToLower().Contains("@" + chat.chat.user.ToLower()) && users.Where(u => response.chat.message.ToLower().Contains("@" + u.userName.ToLower())).Any())
                {
                    maxConfidence = .10;
                }
                else if (!response.chat.message.ToLower().Contains("@" + chat.chat.user.ToLower()) && response.chat.message.Contains("@"))
                {
                    maxConfidence = .20;
                }
                else if (response.chat.message.Count(s => s == '@') > 1)
                {
                    maxConfidence = .20;
                }
            }

            if (chat.chat.time == 0 || response.chat.time == 0)
            {
                return maxConfidence;
            }

            var readTimeMilliseconds = 0.0;
            var responseWordCount = 0;
            if (response.naturalLanguageData.sentences != null)
            {
                responseWordCount = response.naturalLanguageData.sentences.Sum(s => s.Tokens.Count);
                var wordCount = chat.naturalLanguageData.sentences.Sum(s => s.Tokens.Count);
                readTimeMilliseconds = (wordCount / wordsReadPerSecond) * 1000.0;
            }
            var responseTimeMilliseconds = (responseWordCount / wordsTypedPerSecond) * 1000.0;

            var targetTime = readTimeMilliseconds + responseTimeMilliseconds;
            var actualTime = response.chat.time - chat.chat.time;

            var difference = actualTime - targetTime;

            if (difference < 0) //response too fast
            {
                if (actualTime > (minimumResponseTime * 1000))
                {
                    return maxConfidence * .5;
                }
                return 0;
            }

            if (difference > 0)
            {
                if (difference > (maximumResponseTime * 1000)) //response too slow
                {
                    if (response.chat.message.ToLower().Contains(chat.chat.user.ToLower()))
                    {
                        return maxConfidence * .75;
                    }
                    return maxConfidence * .5;
                }
                return maxConfidence;
            }
            return maxConfidence;
        }
    }
}
