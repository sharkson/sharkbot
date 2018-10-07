using ChatModels;

namespace SharkbotApi.Services
{
    public class ResponseRequestValidationService
    {
        public bool ValidRequest(ResponseRequest chat)
        {
            if(string.IsNullOrWhiteSpace(chat.conversationName) || string.IsNullOrWhiteSpace(chat.type))
            {
                return false;
            }
            return true;
        }
    }
}
