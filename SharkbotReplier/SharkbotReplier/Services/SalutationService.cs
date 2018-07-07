using ChatModels;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class SalutationService
    {
        public string GetProperlyAddressedResponse(Conversation analyzedConversation, string response)
        {
            var analyzedChat = analyzedConversation.responses.Last();

            if (analyzedChat.chat.message.Contains("@" + analyzedChat.botName) && !response.Contains(analyzedChat.chat.user))
            {
                response = "@" + analyzedChat.chat.user + " " + response;
            }
            else if (analyzedChat.chat.message.Contains(analyzedChat.botName) && !response.Contains(analyzedChat.chat.user))
            {
                var nickName = analyzedChat.chat.user;
                var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == analyzedChat.chat.user);
                if (userData != null)
                {
                    nickName = userData.nickNames.Last();
                }
                response = nickName + " " + response;
            }
            return response;
        }
    }
}
