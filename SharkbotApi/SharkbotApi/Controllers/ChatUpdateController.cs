using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ChatUpdateController : Controller
    {
        BotService botService;
        RequestValidationService requestValidationService;

        public ChatUpdateController()
        {
            botService = new BotService();
            requestValidationService = new RequestValidationService();
        }

        [HttpPut]
        public bool Put([FromBody]ChatRequest chat)
        {
            if (requestValidationService.ValidRequest(chat))
            {
                return botService.UpdateConversation(chat);
            }
            return false;
        }
    }
}
