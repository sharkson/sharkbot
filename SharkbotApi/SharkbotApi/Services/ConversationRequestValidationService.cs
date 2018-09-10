using ChatModels;

namespace SharkbotApi.Services
{
    public class ConversationRequestValidationService
    {
        public bool ValidRequest(ConversationRequest conversation)
        {
            if(conversation == null || conversation.responses == null || string.IsNullOrWhiteSpace(conversation.name) || string.IsNullOrWhiteSpace(conversation.type))
            {
                return false;
            }
            if(conversation.responses.Count == 0)
            {
                return false;
            }

            foreach(var response in conversation.responses)
            {
                if (string.IsNullOrWhiteSpace(response.chat.message) || string.IsNullOrWhiteSpace(response.chat.user))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
