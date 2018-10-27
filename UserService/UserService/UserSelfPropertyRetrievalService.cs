using ChatModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class UserSelfPropertyRetrievalService
    {
        private UserNaturalLanguageService userNaturalLanguageService;
        private PropertyValueService propertyValueService;

        public UserSelfPropertyRetrievalService()
        {
            userNaturalLanguageService = new UserNaturalLanguageService();
            propertyValueService = new PropertyValueService();
        }

        public ChatResponse GetYourPropertyResponse(AnalyzedChat analyzedChat, UserData userData)
        {
            var requestedPropertyName = getRequestedProperty(analyzedChat);
            if (!string.IsNullOrEmpty(requestedPropertyName))
            {
                var requestedProperty = propertyValueService.getSelfPropertyByValue(requestedPropertyName, userData);
                if (!string.IsNullOrEmpty(requestedProperty.value))
                {
                    var confidence = 1.0;
                    if(requestedProperty.source != userData.userName) //TODO: determine confidence based on source/user relationship, trustworthiness, etc.
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

        private List<string> selfPropertySearch = new List<string>() { "am i (\\p{L}*)", "i'm (\\p{L}*)\\?" };
        private string getRequestedProperty(AnalyzedChat analyzedChat)
        {
            foreach (var regex in selfPropertySearch)
            {
                var match = getPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match) && userNaturalLanguageService.isNaturalLanguageSelfProperty(analyzedChat, match))
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

        private string yourPropertySentence = "You're {0}.";
        private string getYourPropertySentence(UserProperty userProperty)
        {
            return string.Format(yourPropertySentence, userProperty.value);
        }

        public ChatResponse GetOtherPropertyResponse(AnalyzedChat analyzedChat, ConcurrentBag<UserData> users)
        {
            var requestedUserNameAndProperty = getOtherRequestedPropertyName(analyzedChat, users);
            if (!string.IsNullOrEmpty(requestedUserNameAndProperty.userName))
            {
                var confidence = 1.0;
                if(requestedUserNameAndProperty.userProperty.source != requestedUserNameAndProperty.userName)
                {
                    confidence = .75;
                }
                var response = new List<string>();
                response.Add(getOtherPropertySentence(requestedUserNameAndProperty));
                return new ChatResponse { confidence = confidence, response = response };
            }
            return new ChatResponse { confidence = 0, response = new List<string>() };
        }

        private List<string> otherPropertySearch = new List<string>() { "is (\\p{L}*) (\\p{L}*)" };
        private UserNameAndProperty getOtherRequestedPropertyName(AnalyzedChat analyzedChat, ConcurrentBag<UserData> users)
        {
            foreach (var regex in otherPropertySearch)
            {
                var match = getOtherPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && userNaturalLanguageService.isNaturalLanguageSelfProperty(analyzedChat, match.value))
                {
                    var userMatches = users.Where(u => "@" + u.userName == match.name || u.userName == match.name);
                    if (userMatches.Count() == 0)
                    {
                        userMatches = users.Where(u => u.nickNames.Contains(match.name));
                    }
                    if (userMatches.Count() > 0)
                    {
                        var userWithPropertyMatches = userMatches.Where(u => u.properties.Any(p => p.name == "self" && p.value == match.value && p.source == u.userName));
                        if (userWithPropertyMatches.Count() == 0)
                        {
                            userWithPropertyMatches = userMatches.Where(u => u.properties.Any(p => p.name == "self" && p.value == match.value));
                        }
                        if (userWithPropertyMatches.Count() > 0)
                        {
                            var property = userWithPropertyMatches.First().properties.Where(p => p.name == "self" && p.value == match.value).First();
                            return new UserNameAndProperty() { userName = match.name, userProperty = property };
                        }
                    }
                }
            }
            return new UserNameAndProperty() { userName = string.Empty, userProperty = new UserProperty() };
        }

        private UserProperty getOtherPropertyMatch(string source, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                property.name = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
            }
            return property;
        }

        private string otherPropertySentence = "{0} is {1}."; //TODO: XX says XX is XX, if multiple people say it use XX is XX
        private string getOtherPropertySentence(UserNameAndProperty userNameAndProperty)
        {
            return string.Format(otherPropertySentence, userNameAndProperty.userName, userNameAndProperty.userProperty.value);
        }
    }
}
