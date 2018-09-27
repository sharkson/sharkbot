using ChatModels;
using System.Collections.Generic;

namespace SharkbotReplier.Services
{
    public class MathMatchService
    {
        public ChatResponse GetMathMatch(Conversation analyzedConversation)
        {
            return new ChatResponse { confidence = 0, response = new List<string>() }; //TODO: do math, count words, etc.
        }
    }
}