using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace UserService
{
    public class UserPropertyService
    {
        UserNaturalLanguageService userNaturalLanguageService;
        PropertyMatchService propertyMatchService;
        PropertyFromQuestionService propertyFromQuestionService;

        private List<string> propertySearchNameValue = new List<string>() { "my (\\p{L}*) is (\\p{L}*)", "my (\\p{L}*) are (\\p{L}*)" };
        private List<string> propertySearchValueName = new List<string>() { "i have an (\\p{L}*) (\\p{L}*)", "i have a (\\p{L}*) (\\p{L}*)", "i have (\\p{L}*) (\\p{L}*)" };
        private List<string> selfPropertySearchValueName = new List<string>() { "i am a (\\p{L}*)", "i am an (\\p{L}*)", "i am (\\p{L}*)", "i'm a (\\p{L}*)", "i'm an (\\p{L}*)", "i'm (\\p{L}*)" };
        private List<string> excludePropertySearch = new List<string>(); //TODO: not I am going, etc.

        public UserPropertyService()
        {
            userNaturalLanguageService = new UserNaturalLanguageService();
            propertyMatchService = new PropertyMatchService();
            propertyFromQuestionService = new PropertyFromQuestionService();
        }

        public UserProperty GetProperty(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            var property = getPropertyFromResponse(analyzedChat, question);

            if(string.IsNullOrEmpty(property.name))
            {
                property = propertyFromQuestionService.getPropertyFromQuestion(analyzedChat, question);
            }

            return property;
        }

        private UserProperty getPropertyFromResponse(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            var property = new UserProperty();
            if (!excludePropertySearch.Any(e => analyzedChat.chat.message.ToLower().Contains(e)))
            {
                foreach (var regex in propertySearchNameValue)
                {
                    var match = propertyMatchService.getPropertyMatchNameValue(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && userNaturalLanguageService.isNaturalLanguagePropertyName(analyzedChat, match.name) && userNaturalLanguageService.isNaturalLanguagePropertyValue(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
                foreach (var regex in propertySearchValueName)
                {
                    var match = propertyMatchService.getPropertyMatchValueName(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.name) && !string.IsNullOrWhiteSpace(match.value) && userNaturalLanguageService.isNaturalLanguagePropertyName(analyzedChat, match.name) && userNaturalLanguageService.isNaturalLanguagePropertyValue(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
                foreach (var regex in selfPropertySearchValueName)
                {
                    var match = propertyMatchService.getSelfPropertyMatchValue(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match.value) && userNaturalLanguageService.isNaturalLanguageSelfProperty(analyzedChat, match.value))
                    {
                        match.source = analyzedChat.chat.user;
                        match.time = analyzedChat.chat.time;
                        return match;
                    }
                }
            }
            return property;
        }
    }
}
