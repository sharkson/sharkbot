using ChatModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class UserPropertyService
    {
        private List<string> propertySearchNameValue = new List<string>() { "my (\\p{L}*) is (\\p{L}*)", "my (\\p{L}*) are (\\p{L}*)" };
        private List<string> propertySearchValueName = new List<string>() { "i have an (\\p{L}*) (\\p{L}*)", "i have a (\\p{L}*) (\\p{L}*)", "i have (\\p{L}*) (\\p{L}*)" };
        private List<string> selfPropertySearchValueName = new List<string>() { "i am a (\\p{L}*)", "i am an (\\p{L}*)", "i am (\\p{L}*)", "i'm a (\\p{L}*)", "i'm an (\\p{L}*)", "i'm (\\p{L}*)" };
        private List<string> selfPropertySearchUserValueName = new List<string>() { "(\\p{L}*) is (\\p{L}*)" };
        private List<string> propertySearchUserValueName = new List<string>() { "(\\p{L}*) has a (\\p{L}*) (\\p{L}*)", "(\\p{L}*) has an (\\p{L}*) (\\p{L}*)", "(\\p{L}*) has (\\p{L}*) (\\p{L}*)" };
        private List<string> excludePropertySearch = new List<string>();

        public UserProperty GetProperty(AnalyzedChat analyzedChat)
        {
            var property = new UserProperty();
            if (!excludePropertySearch.Any(e => analyzedChat.chat.message.ToLower().Contains(e)))
            {
                foreach (var regex in propertySearchNameValue)
                {
                    var match = getPropertyMatchNameValue(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && isNaturalLanguagePropertyName(analyzedChat, match.name) && isNaturalLanguagePropertyValue(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
                foreach (var regex in propertySearchValueName)
                {
                    var match = getPropertyMatchValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && isNaturalLanguagePropertyName(analyzedChat, match.name) && isNaturalLanguagePropertyValue(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
                foreach (var regex in selfPropertySearchValueName)
                {
                    var match = getSelfPropertyMatchValue(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.value) && isNaturalLanguageSelfProperty(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
            }
            return property;
        }

        public UserNameAndProperty GetOtherUserProperty(AnalyzedChat analyzedChat, List<UserData> users)
        {
            if (!excludePropertySearch.Any(e => analyzedChat.chat.message.ToLower().Contains(e)))
            {
                foreach (var regex in propertySearchUserValueName)
                {
                    var match = getPropertyMatchUserValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.userProperty.name) && !string.IsNullOrWhiteSpace(match.userProperty.value) && isNaturalLanguagePropertyName(analyzedChat, match.userProperty.name) && isNaturalLanguagePropertyValue(analyzedChat, match.userProperty.value))
                    {
                        var userMatches = users.Where(u => "@" + u.userName == match.userName || u.userName == match.userName);
                        if (userMatches.Count() == 0)
                        {
                            userMatches = users.Where(u => u.nickNames.Contains(match.userName));
                        }
                        if (userMatches.Count() > 0)
                        {
                            match.userProperty.source = analyzedChat.chat.user;
                            return match;
                        }
                    }
                }
                foreach (var regex in selfPropertySearchUserValueName)
                {
                    var match = getSelfPropertyMatchUserValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.userProperty.name) && !string.IsNullOrWhiteSpace(match.userProperty.value) && isNaturalLanguageSelfProperty(analyzedChat, match.userProperty.value))
                    {
                        var userMatches = users.Where(u => "@" + u.userName == match.userName || u.userName == match.userName);
                        if (userMatches.Count() == 0)
                        {
                            userMatches = users.Where(u => u.nickNames.Contains(match.userName));
                        }
                        if (userMatches.Count() > 0)
                        {
                            match.userProperty.source = analyzedChat.chat.user;
                            return match;
                        }
                    }
                }
            }
            return new UserNameAndProperty();
        }

        public UserProperty getPropertyByValue(string property, UserData userData)
        {
            var match = userData.properties.Where(p => p.name == property && p.source == userData.userName).LastOrDefault();
            if (match == null)
            {
                match = userData.properties.Where(p => p.name == property).LastOrDefault();
            }
            if (match == null)
            {
                return new UserProperty();
            }
            return match;
        }

        public UserProperty getSelfPropertyByValue(string propertyValue, UserData userData)
        {
            var match = userData.properties.Where(p => p.name == "self" && p.value == propertyValue && p.source == userData.userName).LastOrDefault();
            if (match == null)
            {
                match = userData.properties.Where(p => p.name == "self" && p.value == propertyValue).LastOrDefault();
            }
            if (match == null)
            {
                return new UserProperty();
            }
            return match;
        }

        private UserProperty getPropertyMatchNameValue(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                property.name = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
            }
            return property;
        }

        private UserProperty getPropertyMatchValueName(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                property.value = match.Groups[1].Value;
                property.name = match.Groups[2].Value;
            }
            return property;
        }

        private UserProperty getSelfPropertyMatchValue(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 2)
            {
                property.value = match.Groups[1].Value;
                property.name = "self";
            }
            return property;
        }

        private UserNameAndProperty getPropertyMatchUserValueName(string message, string regex)
        {
            var userName = string.Empty;
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 4)
            {
                userName = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
                property.name = match.Groups[3].Value;
            }
            return new UserNameAndProperty() { userName = userName, userProperty = property };
        }

        private UserNameAndProperty getSelfPropertyMatchUserValueName(string message, string regex)
        {
            var userName = string.Empty;
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                userName = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
                property.name = "self";
            }
            return new UserNameAndProperty() { userName = userName, userProperty = property };
        }

        public bool isNaturalLanguagePropertyName(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.Lexeme == match)
                    {
                        return token.POSTag == "NN" || token.POSTag == "NNP" || token.POSTag == "NNS";
                    }
                }
            }
            return false;
        }

        public bool isNaturalLanguageSelfProperty(AnalyzedChat chat, string match)
        {
            return true; //TODO: probably adjectives
        }

        private bool isNaturalLanguagePropertyValue(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.Lexeme == match)
                    {
                        return token.POSTag == "JJ";
                    }
                }
            }
            return false;
        }
    }
}
