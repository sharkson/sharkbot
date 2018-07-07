using ChatModels;
using System.Linq;
using UserService;

namespace SharkbotReplier.Services
{
    public class UserPropertyMatchService
    {
        UserPropertyRetrievalService userPropertyRetrievalService;
        BotPropertyRetrievalService botPropertyRetrievalService;

        public UserPropertyMatchService()
        {
            userPropertyRetrievalService = new UserPropertyRetrievalService();
            botPropertyRetrievalService = new BotPropertyRetrievalService();
        }

        public ChatResponse GetUserPropertyMatch(Conversation analyzedConversation)
        {
            var analyzedChat = analyzedConversation.responses.Last();
            var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == analyzedChat.chat.user);

            if (userData != null)
            {
                var userResponse = userPropertyRetrievalService.GetYourPropertyResponse(analyzedChat, userData);
                if (!string.IsNullOrEmpty(userResponse.response))
                {
                    return userResponse;
                }
                else
                {
                    var otherResponse = userPropertyRetrievalService.GetOtherPropertyResponse(analyzedChat, UserDatabase.UserDatabase.userDatabase);
                    if (!string.IsNullOrEmpty(otherResponse.response))
                    {
                        return otherResponse;
                    }
                    else
                    {
                        var botData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == analyzedChat.chat.botName);
                        if (botData != null)
                        {
                            var botResponse = botPropertyRetrievalService.GetPropertyResponse(analyzedChat, botData);
                            if (!string.IsNullOrEmpty(botResponse.response))
                            {
                                return botResponse;
                            }
                        }
                    }
                }
            }

            return new ChatResponse { confidence = 0, response = string.Empty };
        }
    }
}