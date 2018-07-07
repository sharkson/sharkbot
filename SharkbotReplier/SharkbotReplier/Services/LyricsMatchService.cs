using ChatModels;

namespace SharkbotReplier.Services
{
    public class LyricsMatchService
    {
        public ChatResponse GetLyricsMatch(Conversation analyzedConversation)
        {
            return new ChatResponse { confidence = 0, response = string.Empty }; //TODO
        }
    }
}