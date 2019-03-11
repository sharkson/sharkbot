using ChatModels;
using System.Collections.Generic;
using System.Linq;
using UserService;

namespace SharkbotReplier.Services
{
    public class ResponseConversionService
    {
        private readonly UserPropertyService _userPropertyService;
        private readonly PropertyValueService _propertyValueService;

        public ResponseConversionService(UserPropertyService userPropertyService, PropertyValueService propertyValueService)
        {
            _userPropertyService = userPropertyService;
            _propertyValueService = propertyValueService;
        }

        public List<string> ConvertResponse(AnalyzedChat target, MatchChat match)
        {
            var responses = new List<string>();
            foreach(var matchChat in match.responseChat)
            {
                var chat = PaddedString(matchChat.chat.message);
                chat = chat.Replace("@" + match.analyzedChat.chat.user, "@" + target.chat.user);

                var originalUser = PaddedString(match.analyzedChat.chat.user);
                var user = PaddedString(target.chat.user);

                chat = chat.Replace(originalUser, user);

                var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == target.chat.user);
                if (userData != null)
                {
                    user = PaddedString(userData.nickNames.Last());
                }

                var originalUserData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == match.analyzedChat.chat.user);
                if (originalUserData != null)
                {
                    foreach (var originalUserName in originalUserData.nickNames)
                    {
                        chat = chat.Replace(PaddedString(originalUserName), user);
                    }
                }

                chat = ConvertProperty(target, match, matchChat, chat, userData);

                responses.Add(chat.Trim());
            }

            return responses;
        }

        private string ConvertProperty(AnalyzedChat target, MatchChat match, AnalyzedChat matchChat, string chat, UserData userData)
        {
            var property = _userPropertyService.GetProperty(matchChat, match.analyzedChat);
            var botUserData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == target.chat.botName);
            if (!string.IsNullOrEmpty(property.name))
            {
                var botProperty = _propertyValueService.getPropertyByValue(property.name, userData);
                if (!string.IsNullOrEmpty(botProperty.value))
                {
                    chat.Replace(property.value, botProperty.value);
                }
            }

            return chat;
        }

        private string PaddedString(string input)
        {
            return " " + input + " ";
        }
    }
}
