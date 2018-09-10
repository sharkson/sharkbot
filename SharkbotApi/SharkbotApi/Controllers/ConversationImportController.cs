using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ConversationImportController : Controller
    {
        BotService botService;
        ConversationRequestValidationService requestValidationService;

        public ConversationImportController()
        {
            botService = new BotService();
            requestValidationService = new ConversationRequestValidationService();
        }

        [HttpPut]
        public bool Put([FromBody]ConversationRequest conversationRequest)
        {
            if (requestValidationService.ValidRequest(conversationRequest))
            {
                var result = botService.UpdateConversation(conversationRequest);

                return result;
            }
            return false;
        }
    }
}
