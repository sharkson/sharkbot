using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ConversationController : Controller
    {
        private readonly ConversationService _conversationService;

        public ConversationController(ConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet("{type}/{conversationName}")]
        public Conversation Get(string type, string conversationName)
        {
            var conversation = _conversationService.GetConversation(conversationName, type);
            return conversation;
        }
    }
}
