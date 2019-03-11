using ChatModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UserService
{
    public class OtherUserPropertyService
    {
        private readonly UserNaturalLanguageService _userNaturalLanguageService;
        private readonly PropertyMatchService _propertyMatchService;

        private List<string> selfPropertySearchUserValueName = new List<string>() { "(\\p{L}*) is (\\p{L}*)" };
        private List<string> propertySearchUserValueName = new List<string>() { "(\\p{L}*) has a (\\p{L}*) (\\p{L}*)", "(\\p{L}*) has an (\\p{L}*) (\\p{L}*)", "(\\p{L}*) has (\\p{L}*) (\\p{L}*)" };
        private List<string> excludePropertySearch = new List<string>();

        public OtherUserPropertyService(UserNaturalLanguageService userNaturalLanguageService, PropertyMatchService propertyMatchService)
        {
            _userNaturalLanguageService = userNaturalLanguageService;
            _propertyMatchService = propertyMatchService;
        }

        public UserNameAndProperty GetOtherUserProperty(AnalyzedChat analyzedChat, ConcurrentBag<UserData> users)
        {
            if (!excludePropertySearch.Any(e => analyzedChat.chat.message.ToLower().Contains(e)))
            {
                foreach (var regex in propertySearchUserValueName)
                {
                    var match = _propertyMatchService.getPropertyMatchUserValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.userProperty.name) && !string.IsNullOrWhiteSpace(match.userProperty.value) && _userNaturalLanguageService.isNaturalLanguagePropertyName(analyzedChat, match.userProperty.name) && _userNaturalLanguageService.isNaturalLanguagePropertyValue(analyzedChat, match.userProperty.value))
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
                    var match = _propertyMatchService.getSelfPropertyMatchUserValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.userProperty.name) && !string.IsNullOrWhiteSpace(match.userProperty.value) && _userNaturalLanguageService.isNaturalLanguageSelfProperty(analyzedChat, match.userProperty.value))
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
    }
}
