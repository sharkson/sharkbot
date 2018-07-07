using ChatModels;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ResponseConversionService
    {
        public string ConvertResponse(AnalyzedChat target, MatchChat match)
        {
            var chat = PaddedString(match.responseChat.chat.message);
            chat = chat.Replace("@" + match.analyzedChat.chat.user, "@" + target.chat.user);

            var originalUser = PaddedString(match.analyzedChat.chat.user);
            var user = PaddedString(target.chat.user);
            
            chat = chat.Replace(originalUser, user);

            var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == target.chat.user);
            if(userData != null)
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

            return chat.Trim();
        }

        private string PaddedString(string input)
        {
            return " " + input + " ";
        }
    }
}
