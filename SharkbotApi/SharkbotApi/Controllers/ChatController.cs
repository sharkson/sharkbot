using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System;
using System.Collections.Generic;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ChatController : Controller
    {
        private readonly QueueService _queueService;
        private readonly ChatRequestValidationService _requestValidationService;

        public ChatController(ChatRequestValidationService requestValidationService, QueueService queueService)
        {
             _requestValidationService = requestValidationService;
            _queueService = queueService;
        }

        [HttpPut]
        public ChatResponse Put([FromBody]ChatRequest chat)
        {
            if(_requestValidationService.ValidRequest(chat))
            {
                chat = cleanRequest(chat);
                if(_queueService.UpdateConversation(chat))
                {
                    var responseRequest = getResponseRequest(chat);
                    var response = _queueService.GetResponse(responseRequest);
                    response.metadata = chat.metadata;
                    return response;
                }
            }

            dynamic metadata = null;
            if(chat != null && chat.metadata != null)
            {
                metadata = chat.metadata;
            }
            return new ChatResponse { confidence = 0, response = new List<string>(), metadata = metadata };
        }

        private ChatRequest cleanRequest(ChatRequest chat)
        {
            if (chat.requiredProperyMatches == null)
            {
                chat.requiredProperyMatches = new List<string>();
            }
            if (chat.exclusiveTypes == null)
            {
                chat.exclusiveTypes = new List<string>();
            }
            if (chat.excludedTypes == null)
            {
                chat.excludedTypes = new List<string>();
            }
            if(chat.subjectGoals == null)
            {
                chat.subjectGoals = new List<string>();
            }

            chat.requestTime = DateTime.Now;

            return chat;
        }

        private ResponseRequest getResponseRequest(ChatRequest chat)
        {
            var responseRequest = new ResponseRequest
            {
                conversationName = chat.conversationName,
                excludedTypes = chat.excludedTypes,
                exclusiveTypes = chat.exclusiveTypes,
                requiredProperyMatches = chat.requiredProperyMatches,
                subjectGoals = chat.subjectGoals,
                type = chat.type,

                requestTime = DateTime.Now
            };

            return responseRequest;
        }
    }
}
