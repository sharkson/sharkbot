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
    public class ResponseController : Controller
    {
        QueueService queueService;
        ResponseRequestValidationService requestValidationService;

        public ResponseController()
        {
            queueService = new QueueService();
            requestValidationService = new ResponseRequestValidationService();
        }

        [HttpPut]
        public ChatResponse Put([FromBody]ResponseRequest chat)
        {
            if (requestValidationService.ValidRequest(chat))
            {
                chat = cleanRequest(chat);
                var response = queueService.GetResponse(chat);

                response.metadata = chat.metadata;
                return response;
            }

            dynamic metadata = null;
            if (chat != null && chat.metadata != null)
            {
                metadata = chat.metadata;
            }
            return new ChatResponse { confidence = 0, response = new List<string>(), metadata = metadata };
        }

        private ResponseRequest cleanRequest(ResponseRequest chat)
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
            if (chat.subjectGoals == null)
            {
                chat.subjectGoals = new List<string>();
            }

            chat.requestTime = DateTime.Now;

            return chat;
        }
    }
}
