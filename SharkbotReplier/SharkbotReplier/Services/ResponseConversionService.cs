using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ResponseConversionService
    {
        //TODO: make sure what's being said isn't a property conflict ex. "I have blue eyes." when the bot has a property of green eyes
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

                responses.Add(chat.Trim());
            }

            return responses;
        }

        private string PaddedString(string input)
        {
            return " " + input + " ";
        }
    }
}
