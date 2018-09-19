using ChatModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharkbotApi.Services;
using System.Linq;

namespace SharkbotApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class UserController : Controller
    {
        BotService botService;
        ChatRequestValidationService requestValidationService;

        public UserController()
        {
            botService = new BotService();
            requestValidationService = new ChatRequestValidationService();
        }

        [HttpGet("{userName}")]
        public UserData Get(string userName)
        {
            var user = UserDatabase.UserDatabase.userDatabase.Where(u => u.userName == userName).FirstOrDefault();
            if(user != null)
            {
                return user;
            }
            return new UserData(userName);
        }
    }
}
