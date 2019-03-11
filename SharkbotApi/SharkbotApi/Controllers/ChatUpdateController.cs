using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ChatUpdateController : Controller
    {
        private readonly QueueService _queueService;
        private readonly ChatRequestValidationService _requestValidationService;

        public ChatUpdateController(ChatRequestValidationService requestValidationService, QueueService queueService)
        {
            _requestValidationService = requestValidationService;
            _queueService = queueService;
        }

        [HttpPut]
        public bool Put([FromBody]ChatRequest chat)
        {
            if (_requestValidationService.ValidRequest(chat))
            {
                chat.requestTime = DateTime.Now;
                return _queueService.UpdateConversation(chat);
            }
            return false;
        }
    }
}
