using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ConversationImportController : Controller
    {
        private readonly QueueService _queueService;
        private readonly ConversationRequestValidationService _requestValidationService;

        public ConversationImportController(ConversationRequestValidationService requestValidationService, QueueService queueService)
        {
            _requestValidationService = requestValidationService;
            _queueService = queueService;
        }

        [HttpPut]
        public bool Put([FromBody]ConversationRequest conversationRequest)
        {
            if (_requestValidationService.ValidRequest(conversationRequest))
            {
                conversationRequest.requestTime = DateTime.Now;
                var result = _queueService.UpdateConversation(conversationRequest);

                return result;
            }
            return false;
        }
    }
}
