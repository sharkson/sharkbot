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
        QueueService queueService;
        ConversationRequestValidationService requestValidationService;

        public ConversationImportController()
        {
            queueService = new QueueService();
            requestValidationService = new ConversationRequestValidationService();
        }

        [HttpPut]
        public bool Put([FromBody]ConversationRequest conversationRequest)
        {
            if (requestValidationService.ValidRequest(conversationRequest))
            {
                conversationRequest.requestTime = DateTime.Now;
                var result = queueService.UpdateConversation(conversationRequest);

                return result;
            }
            return false;
        }
    }
}
