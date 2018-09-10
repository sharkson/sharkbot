using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ChatController : Controller
    {
        BotService botService;
        ChatRequestValidationService requestValidationService;

        public ChatController()
        {
            botService = new BotService();
            requestValidationService = new ChatRequestValidationService();
        }

        [HttpPut]
        public ChatResponse Put([FromBody]ChatRequest chat)
        {
            if(requestValidationService.ValidRequest(chat))
            {
                var response = botService.GetResponse(chat);
                response.metadata = chat.metadata;
                return response;
            }
            dynamic metadata = null;
            if(chat != null && chat.metadata != null)
            {
                metadata = chat.metadata;
            }
            return new ChatResponse { confidence = 0, response = string.Empty, metadata = metadata };
        }
    }
}
