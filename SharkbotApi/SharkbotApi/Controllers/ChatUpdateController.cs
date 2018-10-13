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
        QueueService queueService;
        ChatRequestValidationService requestValidationService;

        public ChatUpdateController()
        {
            queueService = new QueueService();
            requestValidationService = new ChatRequestValidationService();
        }

        [HttpPut]
        public bool Put([FromBody]ChatRequest chat)
        {
            if (requestValidationService.ValidRequest(chat))
            {
                chat.requestTime = DateTime.Now;
                return queueService.UpdateConversation(chat);
            }
            return false;
        }
    }
}
