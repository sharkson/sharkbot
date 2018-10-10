using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using SharkbotApi.Models;
using SharkbotReplier.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SharkbotApi.Services
{
    public class BotService
    {
        private ConversationService conversationService;
        private AnalyzationService analyzationService;
        private ResponseService responseService;
        private ConversationUpdateService covnersationUpdateService;
        private UserService.UserService userService;
        private UpdateDatabasesService updateDatabasesService;

        public BotService()
        {
            conversationService = new ConversationService();
            analyzationService = new AnalyzationService();
            responseService = new ResponseService();
            covnersationUpdateService = new ConversationUpdateService();
            userService = new UserService.UserService();
            updateDatabasesService = new UpdateDatabasesService();
        }

        public ChatResponse GetResponse(ChatRequest chat)
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
                var response = ProcessChat(chat);
                while(!ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem))
                {
                    Debug.WriteLine("dequeue failed");
                }
                return response;
            }
            return Task.Delay(1000).ContinueWith((task) => { return GetResponse(chat); }).Result;
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
                var response = GetChatResponse(conversation, responseRequest.exclusiveTypes, responseRequest.requiredProperyMatches, responseRequest.excludedTypes, responseRequest.subjectGoals);
                while (!ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem))
                {
                    Debug.WriteLine("dequeue failed");
                }
                return response;
            }
            return Task.Delay(1000).ContinueWith((task) => { return GetResponse(responseRequest); }).Result;
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
            return Task.Delay(1000).ContinueWith((task) => { return UpdateConversation(chat); }).Result;
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
            return Task.Delay(1000).ContinueWith((task) => { return UpdateConversation(conversationRequest); }).Result;
        }

        private ChatResponse ProcessChat(ChatRequest chat)
        {
            var conversation = conversationService.GetConversation(chat);
            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);

            AnalyzedChat inResponseTo = null;
            if (analyzedConversation.responses.Count() > 1)
            {
                inResponseTo = analyzedConversation.responses[analyzedConversation.responses.Count() - 2];
            }
            userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return GetChatResponse(conversation, chat.exclusiveTypes, chat.requiredProperyMatches, chat.excludedTypes, chat.subjectGoals);
        }

        private ChatResponse GetChatResponse(Conversation conversation, List<string> exclusiveTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            ChatResponse response;
            if ((exclusiveTypes != null && exclusiveTypes.Count > 0) || (requiredProperyMatches != null && requiredProperyMatches.Count > 0))
            {
                response = responseService.GetResponse(conversation, exclusiveTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            }
            else
            {
                response = responseService.GetResponse(conversation, excludedTypes, subjectGoals);
            }

            return response;
        }
    }
}
