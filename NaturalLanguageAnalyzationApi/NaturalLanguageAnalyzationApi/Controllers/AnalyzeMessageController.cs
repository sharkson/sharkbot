using ChatModels;
using StanfordNaturalLanguageService;
using System.Collections.Generic;
using System.Web.Http;

namespace NaturalLanguageAnalyzationApi.Controllers
{
    public class AnalyzeMessageController : ApiController
    {
        private readonly IMessageAnalyzationService _messageAnalyzationService;

        public AnalyzeMessageController(IMessageAnalyzationService messageAnalyzationService)
        {
            _messageAnalyzationService = messageAnalyzationService;
        }

        public List<Sentence> Get([FromUri] string message)
        {
            var sentences = _messageAnalyzationService.AnalyzeMessage(message);
            return sentences;
        }

        public List<Sentence> Put([FromBody] string message)
        {
            var sentences = _messageAnalyzationService.AnalyzeMessage(message);
            return sentences;
        }
    }
}