using ChatModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UserService
{
    public class BotSelfPropertyRetrievalService
    {
        private UserPropertyService userPropertyService;
        public BotSelfPropertyRetrievalService()
        {
            userPropertyService = new UserPropertyService();
        }

        public ChatResponse GetPropertyResponse(AnalyzedChat analyzedChat, UserData userData)
        {
            var requestedPropertyName = getRequestedProperty(analyzedChat);
            if (!string.IsNullOrEmpty(requestedPropertyName))
            {
                var requestedProperty = userPropertyService.getSelfPropertyByValue(requestedPropertyName, userData);
                if (!string.IsNullOrEmpty(requestedProperty.value))
                {
                    var confidence = 1.0;
                    if(requestedProperty.source != analyzedChat.botName)
                    {
                        confidence = .75;
                    }
                    var response = new List<string>();
                    response.Add(getYourPropertySentence(requestedProperty));
                    return new ChatResponse { confidence = confidence, response = response };
                }
            }
            return new ChatResponse { confidence = 0, response = new List<string>() };
        }

        private List<string> propertySearch = new List<string>() { "are you (\\p{L}*)", "you're (\\p{L}*)?" };
        private string getRequestedProperty(AnalyzedChat analyzedChat)
        {
            foreach (var regex in propertySearch)
            {
                var match = getPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match) && userPropertyService.isNaturalLanguageSelfProperty(analyzedChat, match))
                {
                    return match;
                }
            }
            return string.Empty;
        }

        private string getPropertyMatch(string source, string regex)
        {
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count > 0)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        private string propertySentence = "I am {0}."; //or Yes
        private string getYourPropertySentence(UserProperty userProperty)
        {
            return string.Format(propertySentence, userProperty.value);
        }

        //TODO: I am XX.  Are you XX?
        //I am XX. I'm XX
        //are you XX? you're XX?

    }
}
