using SharkbotApi.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharkbotApi.Services
{
    public static class ConversationTracker
    {
        public static ConcurrentQueue<ConversationQueueItem> requestQueue = new ConcurrentQueue<ConversationQueueItem>();
    }
}
