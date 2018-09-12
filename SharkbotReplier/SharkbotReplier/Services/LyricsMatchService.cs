using ChatModels;
using System.Collections.Generic;

namespace SharkbotReplier.Services
{
    public class LyricsMatchService
    {
        public ChatResponse GetLyricsMatch(Conversation analyzedConversation)
        {
            return new ChatResponse { confidence = 0, response = new List<string>() }; //TODO
        }
    }
}