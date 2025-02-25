﻿using ChatModels;
using SharkbotApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharkbotApi.Services
{
    public class QueueService
    {
        private readonly BotService _botService;
        private readonly ConversationService _conversationService;
        private readonly UpdateDatabasesService _updateDatabasesService;
        private readonly StalkerService _stalkerService;

        private readonly int queueDelay = 500;
        private readonly int maximumDelay = 10000;

        public QueueService(BotService botService, ConversationService conversationService, UpdateDatabasesService updateDatabasesService, StalkerService stalkerService)
        {
            _botService =botService;
            _conversationService =conversationService;
            _updateDatabasesService = updateDatabasesService;
            _stalkerService = stalkerService;
        }

        public ChatResponse GetResponse(ResponseRequest responseRequest)
        {
            if (responseRequest.requestTime == null)
            {
                responseRequest.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = responseRequest.conversationName, RequestTime = responseRequest.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
                {
                    var conversation = _conversationService.GetConversation(responseRequest.conversationName, responseRequest.type);
                    var response = _botService.GetChatResponse(conversation, responseRequest.exclusiveTypes, responseRequest.requiredProperyMatches, responseRequest.excludedTypes, responseRequest.subjectGoals);
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);

                    return response;
                }
                CleanQueue();
            }
            return Task.Delay(queueDelay).ContinueWith((task) => { return GetResponse(responseRequest); }).Result;
        }

        public ChatResponse GetReaction(ResponseRequest responseRequest) //TODO:
        {
            if (responseRequest.requestTime == null)
            {
                responseRequest.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = responseRequest.conversationName, RequestTime = responseRequest.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
                {
                    var conversation = _conversationService.GetConversation(responseRequest.conversationName, responseRequest.type);
                    var response = _botService.GetChatReaction(conversation, responseRequest.exclusiveTypes, responseRequest.requiredProperyMatches, responseRequest.excludedTypes, responseRequest.subjectGoals);
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);

                    return response;
                }
                CleanQueue();
            }
            return Task.Delay(queueDelay).ContinueWith((task) => { return GetResponse(responseRequest); }).Result;
        }

        public bool UpdateConversation(ChatRequest chat)
        {
            if (chat.requestTime == null)
            {
                chat.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = chat.conversationName, RequestTime = chat.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
                {
                    var updated = _updateDatabasesService.UpdateDatabases(chat);
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
                    _stalkerService.StalkUser(chat.chat.user);

                    return updated;
                }
                CleanQueue();
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
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
                {
                    var updated = _updateDatabasesService.UpdateDatabases(conversationRequest);
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
                    return updated;
                }
                CleanQueue();
            }

            return Task.Delay(queueDelay).ContinueWith((task) => { return UpdateConversation(conversationRequest); }).Result;
        }

        public bool UpdateConversation(ReactionRequest reactionRequest)
        {
            if (reactionRequest.requestTime == null)
            {
                reactionRequest.requestTime = DateTime.Now;
            }

            var queueItem = new ConversationQueueItem { ConversationName = reactionRequest.conversationName, RequestTime = reactionRequest.requestTime };

            if (!ConversationTracker.requestQueue.Any(i => i.ConversationName == queueItem.ConversationName && i.RequestTime == queueItem.RequestTime))
            {
                ConversationTracker.requestQueue.Enqueue(queueItem);
            }

            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.ConversationName == queueItem.ConversationName && peekedQueueItem.RequestTime == queueItem.RequestTime)
                {
                    var updated = _updateDatabasesService.UpdateDatabases(reactionRequest);
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);

                    return updated;
                }
                CleanQueue();
            }

            return Task.Delay(queueDelay).ContinueWith((task) => { return UpdateConversation(reactionRequest); }).Result;
        }

        private void CleanQueue()
        {
            ConversationQueueItem peekedQueueItem;
            if (ConversationTracker.requestQueue.TryPeek(out peekedQueueItem))
            {
                if (peekedQueueItem.RequestTime.Value.AddMilliseconds(maximumDelay) < DateTime.Now)
                {
                    ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
                    CleanQueue();
                }
            }
        }
    }
}
