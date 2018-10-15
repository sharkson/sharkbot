using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ResponseService
    {
        private ConversationMatchService conversationMatchService;
        private UserPropertyMatchService userPropertyService;
        private LyricsMatchService lyricsService;
        private GoogleMatchService.GoogleMatchService googleService;
        private UrbanDictionaryMatchService urbanDictionaryService;
        private SalutationService salutationService;
        private ResponseConversionService responseConversionService;

        private const double googleThreshold = .9;
        private const double urbanDictionaryThreshold = .9;

        public ResponseService()
        {
            conversationMatchService = new ConversationMatchService();
            userPropertyService = new UserPropertyMatchService();
            lyricsService = new LyricsMatchService();
            googleService = new GoogleMatchService.GoogleMatchService();
            urbanDictionaryService = new UrbanDictionaryMatchService();
            salutationService = new SalutationService();
            responseConversionService = new ResponseConversionService();
        }

        public ChatResponse GetResponse(Conversation analyzedConversation, List<string> excludedTypes, List<string> subjectGoals)
        {
            if (analyzedConversation.responses.Count == 0)
            {
                return new ChatResponse() { confidence = 0, response = new List<string>() };
            }

            var conversationChatResponse = conversationMatchService.GetConversationMatch(analyzedConversation, excludedTypes, subjectGoals);
 
            var userPropertyChatResponse = userPropertyService.GetUserPropertyMatch(analyzedConversation);

            var lyricsChatResponse = lyricsService.GetLyricsMatch(analyzedConversation);

            //TODO: media comment match
            //if it's a youtube video look up comments on the video and use one, soundcloud comments, reddit post comments, if it's a tweet look up replies to the tweet, etc.

            var googleChatResponse = googleService.GetGoogleMatch(analyzedConversation);

            var urbanDictionaryChatResponse = urbanDictionaryService.GetUrbanDictionaryMatch(analyzedConversation);

            //TODO: run all matches simultaneously then decide which one to use

            var matchChat = conversationChatResponse;

            if(userPropertyChatResponse.confidence > conversationChatResponse.matchConfidence) //TODO: check the uniqueness of reply (if it was already used)
            {
                userPropertyChatResponse.response = salutationService.GetProperlyAddressedResponse(analyzedConversation, userPropertyChatResponse.response);
                return userPropertyChatResponse;
            }

            if(conversationChatResponse.matchConfidence < googleThreshold && googleChatResponse.confidence > conversationChatResponse.matchConfidence)
            {
                googleChatResponse.response = salutationService.GetProperlyAddressedResponse(analyzedConversation, googleChatResponse.response);
                return googleChatResponse;
            }
            if (conversationChatResponse.matchConfidence < urbanDictionaryThreshold && urbanDictionaryChatResponse.confidence > conversationChatResponse.matchConfidence)
            {
                urbanDictionaryChatResponse.response = salutationService.GetProperlyAddressedResponse(analyzedConversation, urbanDictionaryChatResponse.response);
                return urbanDictionaryChatResponse;
            }

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var response = responseConversionService.ConvertResponse(analyzedConversation.responses.Last(), matchChat);

            //TODO: alter reply to match sophistication

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = salutationService.GetProperlyAddressedResponse(analyzedConversation, response)
            };

            return chatResponse;
        }

        public ChatResponse GetResponse(Conversation analyzedConversation, List<string> requiredTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            //TODO: change type to list of types, pass that in. if it's empty do any
            var conversationChatResponse = conversationMatchService.GetConversationMatch(analyzedConversation, requiredTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            var userPropertyChatResponse = userPropertyService.GetUserPropertyMatch(analyzedConversation);

            var matchChat = conversationChatResponse;

            if (userPropertyChatResponse.confidence > conversationChatResponse.matchConfidence) //TODO: check the uniqueness of reply (if it was already used)
            {
                userPropertyChatResponse.response = salutationService.GetProperlyAddressedResponse(analyzedConversation, userPropertyChatResponse.response);
                return userPropertyChatResponse;
            }

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var response = responseConversionService.ConvertResponse(analyzedConversation.responses.Last(), matchChat);

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = salutationService.GetProperlyAddressedResponse(analyzedConversation, response)
            };

            //TODO: alter reply to match sophistication

            return chatResponse;
        }
    }
}
