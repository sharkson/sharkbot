using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class SalutationService
    {
        public List<string> GetProperlyAddressedResponse(Conversation analyzedConversation, List<string> responses)
        {
            var analyzedChat = analyzedConversation.responses.Last();

            if(responses.Count > 0)
            {
                if (analyzedChat.chat.message.Contains("@" + analyzedChat.botName) && !responses[0].Contains(analyzedChat.chat.user))
                {
                    responses[0] = "@" + analyzedChat.chat.user + " " + responses[0];
                }
                else if (analyzedChat.chat.message.Contains(analyzedChat.botName) && !responses[0].Contains(analyzedChat.chat.user))
                {
                    var nickName = analyzedChat.chat.user;
                    var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == analyzedChat.chat.user);
                    if (userData != null)
                    {
                        nickName = userData.nickNames.Last();
                    }
                    responses[0] = nickName + " " + responses[0];
                }
            }
            
            return responses;
        }
    }
}
