using ChatModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UserService
{
    public class BotPropertyRetrievalService
    {
        private UserPropertyService userPropertyService;
        private BotSelfPropertyRetrievalService botSelfPropertyRetrievalService;

        public BotPropertyRetrievalService()
        {
            userPropertyService = new UserPropertyService();
            botSelfPropertyRetrievalService = new BotSelfPropertyRetrievalService();
        }

        public ChatResponse GetPropertyResponse(AnalyzedChat analyzedChat, UserData userData)
        {
            var requestedPropertyName = getRequestedPropertyName(analyzedChat);
            if (!string.IsNullOrEmpty(requestedPropertyName))
            {
                var requestedProperty = userPropertyService.getPropertyByValue(requestedPropertyName, userData);
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
            return botSelfPropertyRetrievalService.GetPropertyResponse(analyzedChat, userData);
        }

        private List<string> propertySearch = new List<string>() { "is your (\\p{L}*)", "are your (\\p{L}*)" };
        private string getRequestedPropertyName(AnalyzedChat analyzedChat)
        {
            foreach (var regex in propertySearch)
            {
                var match = getPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match) && userPropertyService.isNaturalLanguagePropertyName(analyzedChat, match))
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

        private string propertySentence = "My {0} {1} {2}.";
        private string getYourPropertySentence(UserProperty userProperty)
        {
            var verb = "is";
            if (userProperty.name.EndsWith("s")) //TODO: use more advanced naturallanguage to determine if plural, deer could be plural
            {
                verb = "are";
            }
            return string.Format(propertySentence, userProperty.name, verb, userProperty.value);
        }
    }
}
