using ChatModels;

namespace SharkbotApi.Services
{
    public class ReactionRequestValidationService
    {
        public bool ValidRequest(ReactionRequest request)
        {
            if(request == null || request.reaction == null || request.chat == null || string.IsNullOrWhiteSpace(request.conversationName) || string.IsNullOrWhiteSpace(request.type))
            {
                return false;
            }
            if(string.IsNullOrWhiteSpace(request.reaction.reaction) || string.IsNullOrWhiteSpace(request.reaction.user))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(request.chat.message) || string.IsNullOrWhiteSpace(request.chat.user))
            {
                return false;
            }

            return true;
        }
    }
}
