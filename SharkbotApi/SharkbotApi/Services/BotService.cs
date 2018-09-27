using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using SharkbotApi.Models;
using SharkbotReplier.Services;
using System;
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

        public BotService()
        {
            conversationService = new ConversationService();
            analyzationService = new AnalyzationService();
            responseService = new ResponseService();
            covnersationUpdateService = new ConversationUpdateService();
            userService = new UserService.UserService();
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
                ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
                return response;
            }
            return Task.Delay(1000).ContinueWith((task) => { return GetResponse(chat); }).Result;
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
                var updated = UpdateDatabases(chat);
                ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
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
                var updated = UpdateDatabases(conversationRequest);
                ConversationTracker.requestQueue.TryDequeue(out peekedQueueItem);
                return updated;
            }
            return Task.Delay(1000).ContinueWith((task) => { return UpdateConversation(conversationRequest); }).Result;
        }

        private ChatResponse ProcessChat(ChatRequest chat)
        {
            var conversation = conversationService.GetConversation(chat);
            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var inResponseTo = analyzedConversation.responses.LastOrDefault();

            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);

            userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            ChatResponse response;
            if ((chat.exclusiveTypes != null && chat.exclusiveTypes.Count > 0) || (chat.requiredProperyMatches != null && chat.requiredProperyMatches.Count > 0))
            {
                response = responseService.GetResponse(analyzedConversation, chat.exclusiveTypes, chat.requiredProperyMatches, chat.excludedTypes);
            }
            else
            {
                response = responseService.GetResponse(analyzedConversation, chat.excludedTypes);
            }

            return response;
        }

        private bool UpdateDatabases(ChatRequest chat)
        {
            var conversation = conversationService.GetConversation(chat);
            var inResponseTo = conversation.responses.LastOrDefault();
            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, chat.type);
            userService.UpdateUsers(analyzedConversation.responses.Last(), inResponseTo);

            return conversationUdpdated;
        }

        private bool UpdateDatabases(ConversationRequest conversationRequest)
        {
            var conversation = new Conversation
            {
                name = conversationRequest.name,
                responses = conversationRequest.responses
            };

            var analyzedConversation = analyzationService.AnalyzeConversation(conversation);
            var conversationUdpdated = covnersationUpdateService.UpdateConversation(analyzedConversation, conversationRequest.type);

            AnalyzedChat previousChat = null;
            foreach(var analyziedChat in analyzedConversation.responses)
            {
                userService.UpdateUsers(analyziedChat, previousChat);
                previousChat = analyziedChat;
            }

            return conversationUdpdated;
        }
    }
}
