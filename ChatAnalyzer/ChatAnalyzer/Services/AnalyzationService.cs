
using ChatModels;
using NaturalLanguageService.Services;
using SharkbotConfiguration;
using System.Collections.Generic;
using System.Linq;

namespace ChatAnalyzer.Services
{
    public class AnalyzationService
    {
        private ConversationSubjectService conversationSubjectService;
        private ResponseAnalyzationService responseAnalyzationService;
        private ConversationTypeService conversationTypeService;
        private UserlessMessageService userlessMessageService;
        private ConversationReadingLevelService conversationReadingLevelService;

        public AnalyzationService()
        {
            conversationSubjectService = new ConversationSubjectService();
            responseAnalyzationService = new ResponseAnalyzationService();
            conversationTypeService = new ConversationTypeService();
            userlessMessageService = new UserlessMessageService();
            conversationReadingLevelService = new ConversationReadingLevelService();
        }

        public Conversation AnalyzeConversation(Conversation conversation)
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
                response.naturalLanguageData = NaturalLanguageService.NaturalLanguageService.AnalyzeMessage(response.chat);
                response.naturalLanguageData.userlessMessage = userlessMessageService.GetMessageWithoutUsers(response.chat.message, users);
            }

            conversation.groupChat = conversationTypeService.GetConversationGroupChatType(conversation.responses);

            for (var index = 0; index < conversation.responses.Count; index++)
            {
                conversation.responses[index].naturalLanguageData.responseConfidence = 0;

                if (conversation.responses.Count > index + 1)
                {
                    var nextResponse = conversation.responses[index + 1];
                    if (responseAnalyzationService.MessageHasUsableResponse(conversation.responses[index].chat, nextResponse.chat))
                    {
                        conversation.responses[index].naturalLanguageData.responseConfidence = responseAnalyzationService.getReplyConfidence(conversation.responses[index], nextResponse, UserDatabase.UserDatabase.userDatabase, conversation.groupChat);
                    }
                }

                conversation.responses[index].naturalLanguageData.proximitySubjects = conversationSubjectService.GetProximitySubjects(conversation, index);
            }

            conversation.subjects = conversationSubjectService.GetConversationSubjects(conversation.responses);

            conversation.readingLevel = conversationReadingLevelService.GetReadingLevel(conversation.responses);

            conversation.analyzationVersion = ConfigurationService.AnalyzationVersion;

            return conversation;
        }
    }
}
