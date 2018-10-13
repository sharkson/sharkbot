using ChatModels;
using SharkbotApi.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SharkbotApi.Services
{
    public class QueueService
    {
        BotService botService;
        ConversationService conversationService;
        private UpdateDatabasesService updateDatabasesService;

        int queueDelay = 1000;

        public QueueService()
        {
            botService = new BotService();
            conversationService = new ConversationService();
            updateDatabasesService = new UpdateDatabasesService();
        }

        public ChatResponse GetResponse(ResponseRequest responseRequest)
        {
            if (responseRequest.requestTime == null)
            {
                responseRequest.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = responseRequest.conversationName, RequestTime = (DateTime)responseRequest.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem) && peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
            {
                var conversation = conversationService.GetConversation(responseRequest.conversationName, responseRequest.type);
                var response = botService.GetChatResponse(conversation, responseRequest.exclusiveTypes, responseRequest.requiredProperyMatches, responseRequest.excludedTypes, responseRequest.subjectGoals);
                while (!ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem))
                {
                    Debug.WriteLine("dequeue failed");
                }
                return response;
            }
            return Task.Delay(queueDelay).ContinueWith((task) => { return GetResponse(responseRequest); }).Result;
        }

        public bool UpdateConversation(ChatRequest chat)
        {
            if (chat.requestTime == null)
            {
                chat.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = chat.conversationName, RequestTime = (DateTime)chat.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem) && peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
            {
                var updated = updateDatabasesService.UpdateDatabases(chat);
                while (!ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem))
                {
                    Debug.WriteLine("dequeue failed");
                }
                return updated;
            }
            return Task.Delay(queueDelay).ContinueWith((task) => { return UpdateConversation(chat); }).Result;
        }

        public bool UpdateConversation(ConversationRequest conversationRequest)
        {
            if (conversationRequest.requestTime == null)
            {
                conversationRequest.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = conversationRequest.name, RequestTime = conversationRequest.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem) && peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
            {
                var updated = updateDatabasesService.UpdateDatabases(conversationRequest);
                while (!ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem))
                {
                    Debug.WriteLine("dequeue failed");
                }
                return updated;
            }
            return Task.Delay(queueDelay).ContinueWith((task) => { return UpdateConversation(conversationRequest); }).Result;
        }
    }
}
