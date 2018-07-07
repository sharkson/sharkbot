using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NaturalLanguageService.Services
{
    public class UserlessMessageService
    {
        public string GetMessageWithoutUsers(string originalMessage, List<string> users) //TODO: don't remove a username if it isn't being used as a username and is just a common word ex. username is House and message is I live in a house.
        {
            var message = PaddedString(originalMessage);

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    message = Regex.Replace(message, "@" + Regex.Escape(user), string.Empty, RegexOptions.IgnoreCase);
                }
            }
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    message = Regex.Replace(message, Regex.Escape(PaddedString(user)), " ", RegexOptions.IgnoreCase);

                    var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == user);
                    if(userData != null)
                    {
                        foreach(var nickName in userData.nickNames)
                        {
                            message = Regex.Replace(message, Regex.Escape(PaddedString(nickName)), " ", RegexOptions.IgnoreCase);
                        }
                    }
                }
            }

            return message.Trim();
        }

        private string PaddedString(string input)
        {
            return " " + input + " ";
        }
    }
}
