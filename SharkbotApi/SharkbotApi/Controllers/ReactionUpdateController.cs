using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ReactionUpdateController : Controller
    {
        private readonly QueueService _queueService;
        private readonly ReactionRequestValidationService _requestValidationService;

        public ReactionUpdateController(ReactionRequestValidationService requestValidationService, QueueService queueService)
        {
            _requestValidationService = requestValidationService;
            _queueService = queueService;
        }

        [HttpPut]
        public bool Put([FromBody]ReactionRequest reaction)
        {
            if (_requestValidationService.ValidRequest(reaction))
            {
                reaction.requestTime = DateTime.Now;
                return _queueService.UpdateConversation(reaction);
            }
            return false;
        }
    }
}
