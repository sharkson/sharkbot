﻿using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System;
using System.Collections.Generic;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ReactionController : Controller
    {
        private readonly QueueService _queueService;
        private readonly ResponseRequestValidationService _requestValidationService;

        public ReactionController(ResponseRequestValidationService requestValidationService, QueueService queueService)
        {
            _requestValidationService = requestValidationService;
            _queueService = queueService;
        }

        [HttpPut]
        public ChatResponse Put([FromBody]ResponseRequest chat)
        {
            if (_requestValidationService.ValidRequest(chat))
            {
                chat = cleanRequest(chat);
                var response = _queueService.GetReaction(chat);

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
