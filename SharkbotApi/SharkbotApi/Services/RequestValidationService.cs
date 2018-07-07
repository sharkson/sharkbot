using ChatModels;

namespace SharkbotApi.Services
{
    public class RequestValidationService
    {
        public bool ValidRequest(ChatRequest chat)
        {
            if(chat == null || chat.chat == null || string.IsNullOrWhiteSpace(chat.conversationName) || string.IsNullOrWhiteSpace(chat.type))
            {
                return false;
            }
            if(string.IsNullOrWhiteSpace(chat.chat.message) || string.IsNullOrWhiteSpace(chat.chat.user))
            {
                return false;
            }

            return true;
        }
    }
}
