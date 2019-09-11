
using ChatModels;
using NaturalLanguageService.Services;
using SharkbotConfiguration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAnalyzer.Services
{
    public class AnalyzationService
    {
        private readonly ConversationSubjectService _conversationSubjectService;
        private readonly ResponseAnalyzationService _responseAnalyzationService;
        private readonly ConversationTypeService _conversationTypeService;
        private readonly UserlessMessageService _userlessMessageService;
        private readonly ConversationReadingLevelService _conversationReadingLevelService;
        private readonly ResponseSubjectService _responseSubjectService;
        private readonly NaturalLanguageApiService _naturalLanguageApiService;

        public AnalyzationService(ConversationSubjectService conversationSubjectService, ResponseAnalyzationService responseAnalyzationService, ConversationTypeService conversationTypeService, UserlessMessageService userlessMessageService, ConversationReadingLevelService conversationReadingLevelService, ResponseSubjectService responseSubjectService, NaturalLanguageApiService naturalLanguageApiService)
        {
            _conversationSubjectService = conversationSubjectService;
            _responseAnalyzationService = responseAnalyzationService;
            _conversationTypeService =conversationTypeService;
            _userlessMessageService = userlessMessageService;
            _conversationReadingLevelService =conversationReadingLevelService;
            _responseSubjectService = responseSubjectService;
            _naturalLanguageApiService = naturalLanguageApiService;
        }

        public Conversation AnalyzeConversationAsync(Conversation conversation)
        {
            var users = conversation.responses.Select(r => r.chat.user).Distinct().ToList();

            foreach (var response in conversation.responses)
            {
                if(string.IsNullOrWhiteSpace(response.chat.user))
                {
                    response.chat.user = "unkown";
                }
                if (string.IsNullOrWhiteSpace(response.chat.message))
                {
                    response.chat.message = " ";
                }
                if(response.naturalLanguageData == null || response.naturalLanguageData.AnalyzationVersion != ConfigurationService.AnalyzationVersion)
                {
                    response.naturalLanguageData = new NaturalLanguageData { AnalyzationVersion = ConfigurationService.AnalyzationVersion };

                    try
                    {
                        var result = Task.Run(async () => await _naturalLanguageApiService.AnalyzeMessageAsync(response.chat.message)).ConfigureAwait(false);
                        response.naturalLanguageData.sentences = result.GetAwaiter().GetResult();
                    }
                    catch(Exception)
                    {

                    }
                }

                response.naturalLanguageData.userlessMessage = _userlessMessageService.GetMessageWithoutUsers(response.chat.message, users);
                response.naturalLanguageData.subjects = _responseSubjectService.GetSubjects(response);
            }

            conversation.groupChat = _conversationTypeService.GetConversationGroupChatType(conversation.responses);

            for (var index = 0; index < conversation.responses.Count; index++)
            {
                conversation.responses[index].naturalLanguageData.responseConfidence = 0;

                if (conversation.responses.Count > index + 1)
                {
                    var nextResponse = conversation.responses[index + 1];
                    if (_responseAnalyzationService.MessageHasUsableResponse(conversation.responses[index].chat, nextResponse.chat))
                    {
                        conversation.responses[index].naturalLanguageData.responseConfidence = _responseAnalyzationService.GetReplyConfidence(conversation.responses[index], nextResponse, UserDatabase.UserDatabase.userDatabase, conversation.groupChat);
                        conversation.responses[index].naturalLanguageData.responseSubjects = nextResponse.naturalLanguageData.subjects;
                    }
                }

                conversation.responses[index].naturalLanguageData.proximitySubjects = _conversationSubjectService.GetProximitySubjects(conversation, index);
            }

            conversation.subjects = _conversationSubjectService.GetConversationSubjects(conversation.responses);

            conversation.readingLevel = _conversationReadingLevelService.GetReadingLevel(conversation.responses);

            conversation.analyzationVersion = ConfigurationService.AnalyzationVersion;

            return conversation;
        }
    }
}
