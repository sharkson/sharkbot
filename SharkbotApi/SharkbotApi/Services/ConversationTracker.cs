using SharkbotApi.Models;
using System.Collections.Generic;

namespace SharkbotApi.Services
{
    public static class ConversationTracker
    {
        public static List<ConversationQueueItem> activeConversationNames = new List<ConversationQueueItem>();
    }
}
