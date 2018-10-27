using ChatModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class UserPropertyRetrievalService
    {
        private PropertyValueService propertyValueService;
        private UserSelfPropertyRetrievalService userSelfPropertyRetrievalService;
        private UserNaturalLanguageService userNaturalLanguageService;

        public UserPropertyRetrievalService()
        {
            propertyValueService = new PropertyValueService();
            userSelfPropertyRetrievalService = new UserSelfPropertyRetrievalService();
            userNaturalLanguageService = new UserNaturalLanguageService();
        }

        public ChatResponse GetYourPropertyResponse(AnalyzedChat analyzedChat, UserData userData)
        {
            var requestedPropertyName = getSelfRequestedPropertyName(analyzedChat);
            if (!string.IsNullOrEmpty(requestedPropertyName))
            {
                var requestedProperty = propertyValueService.getPropertyByValue(requestedPropertyName, userData);
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
            return userSelfPropertyRetrievalService.GetYourPropertyResponse(analyzedChat, userData);
        }

        private List<string> selfPropertySearch = new List<string>() { "is my (\\p{L}*)", "are my (\\p{L}*)" };
        private string getSelfRequestedPropertyName(AnalyzedChat analyzedChat)
        {
            foreach (var regex in selfPropertySearch)
            {
                var match = getSelfPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match) && userNaturalLanguageService.isNaturalLanguagePropertyName(analyzedChat, match))
                {
                    return match;
                }
            }
            return string.Empty;
        }

        private string getSelfPropertyMatch(string source, string regex)
        {
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count > 0)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        private string yourPropertySentence = "Your {0} {1} {2}.";
        private string getYourPropertySentence(UserProperty userProperty)
        {
            var verb = "is";
            if (userProperty.name.EndsWith("s")) //TODO: use naturallanguage to determine if plural
            {
                verb = "are";
            }
            return string.Format(yourPropertySentence, userProperty.name, verb, userProperty.value);
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
            return userSelfPropertyRetrievalService.GetOtherPropertyResponse(analyzedChat, users);
        }

        //TODO: who is * //search nicknames and give username.  * is @username, if say name instead of nickname say some properties of that user, check if user is in property of another user
        private List<string> otherPropertySearch = new List<string>() { "is (\\p{L}*)'s (\\p{L}*)", "are (\\p{L}*)'s (\\p{L}*)" };
        private UserNameAndProperty getOtherRequestedPropertyName(AnalyzedChat analyzedChat, ConcurrentBag<UserData> users)
        {
            foreach (var regex in otherPropertySearch)
            {
                var match = getOtherPropertyMatch(analyzedChat.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && userNaturalLanguageService.isNaturalLanguagePropertyName(analyzedChat, match.value))
                {
                    var userMatches = users.Where(u => "@" + u.userName == match.name || u.userName == match.name);
                    if (userMatches.Count() == 0)
                    {
                        userMatches = users.Where(u => u.nickNames.Contains(match.name));
                    }
                    if (userMatches.Count() > 0)
                    {
                        var userWithPropertyMatches = userMatches.Where(u => u.properties.Any(p => p.name == match.value && p.source == u.userName));
                        if (userWithPropertyMatches.Count() == 0)
                        {
                            userWithPropertyMatches = userMatches.Where(u => u.properties.Any(p => p.name == match.value));
                        }
                        if (userWithPropertyMatches.Count() > 0)
                        {
                            var property = userWithPropertyMatches.First().properties.Where(p => p.name == match.value).First();
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

        private string otherPropertySentence = "{0}'s {1} {2} {3}.";
        private string getOtherPropertySentence(UserNameAndProperty userNameAndProperty)
        {
            var verb = "is";
            if (userNameAndProperty.userProperty.name.EndsWith("s")) //TODO: use naturallanguage to determine if plural
            {
                verb = "are";
            }
            return string.Format(otherPropertySentence, userNameAndProperty.userName, userNameAndProperty.userProperty.name, verb, userNameAndProperty.userProperty.value);
        }

        //TODO: If xx xx then yy yy.
        // xx xx.
        // yy yy.

        //If I win then you lose.
        //I win.
        //I lose.

        //What happens if I win?
        //I lose

        //match converted tense.  Won, win, will win
    }
}
