using ChatModels;
using System.Collections.Generic;
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
                if (userResponse.response.Count > 0)
                {
                    return userResponse;
                }
                else
                {
                    var otherResponse = userPropertyRetrievalService.GetOtherPropertyResponse(analyzedChat, UserDatabase.UserDatabase.userDatabase);
                    if (otherResponse.response.Count > 0)
                    {
                        return otherResponse;
                    }
                    else
                    {
                        var botData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(u => u.userName == analyzedChat.chat.botName);
                        if (botData != null)
                        {
                            var botResponse = botPropertyRetrievalService.GetPropertyResponse(analyzedChat, botData);
                            if (botResponse.response.Count > 0)
                            {
                                return botResponse;
                            }
                        }
                    }
                }
            }

            return new ChatResponse { confidence = 0, response = new List<string>() };
        }
    }
}