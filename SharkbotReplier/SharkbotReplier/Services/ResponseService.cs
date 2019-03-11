using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class ResponseService
    {
        private readonly ConversationMatchService _conversationMatchService;
        private readonly UserPropertyMatchService _userPropertyMatchService;
        private readonly LyricsMatchService _lyricsService;
        private readonly GoogleMatchService.GoogleMatchService _googleService;
        private readonly UrbanDictionaryMatchService _urbanDictionaryService;
        private readonly SalutationService _salutationService;
        private readonly ResponseConversionService _responseConversionService;

        private const double googleThreshold = .9;
        private const double urbanDictionaryThreshold = .9;

        public ResponseService(ConversationMatchService conversationMatchService, UserPropertyMatchService userPropertyMatchService, LyricsMatchService lyricsService, GoogleMatchService.GoogleMatchService googleService, UrbanDictionaryMatchService urbanDictionaryService, SalutationService salutationService, ResponseConversionService responseConversionService)
        {
            _conversationMatchService = conversationMatchService;
            _userPropertyMatchService = userPropertyMatchService;
            _lyricsService = lyricsService;
            _googleService = googleService;
            _urbanDictionaryService = urbanDictionaryService;
            _salutationService = salutationService;
            _responseConversionService = responseConversionService;
        }

        public ChatResponse GetResponse(Conversation analyzedConversation, List<string> excludedTypes, List<string> subjectGoals)
        {
            if (analyzedConversation.responses.Count == 0)
            {
                return new ChatResponse() { confidence = 0, response = new List<string>() };
            }

            var conversationChatResponse = _conversationMatchService.GetConversationMatch(analyzedConversation, excludedTypes, subjectGoals);
 
            var userPropertyChatResponse = _userPropertyMatchService.GetUserPropertyMatch(analyzedConversation);

            var lyricsChatResponse = _lyricsService.GetLyricsMatch(analyzedConversation);

            //TODO: media comment match
            //if it's a youtube video look up comments on the video and use one, soundcloud comments, reddit post comments, if it's a tweet look up replies to the tweet, etc.

            var googleChatResponse = _googleService.GetGoogleMatch(analyzedConversation);

            var urbanDictionaryChatResponse = _urbanDictionaryService.GetUrbanDictionaryMatch(analyzedConversation);

            //TODO: run all matches simultaneously then decide which one to use, set a maximum time to wait for each one, ignore it if it takes too long

            var matchChat = conversationChatResponse;

            if(userPropertyChatResponse.confidence > conversationChatResponse.matchConfidence) //TODO: check the uniqueness of reply (if it was already used)
            {
                userPropertyChatResponse.response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, userPropertyChatResponse.response);
                return userPropertyChatResponse;
            }

            if(conversationChatResponse.matchConfidence < googleThreshold && googleChatResponse.confidence > conversationChatResponse.matchConfidence)
            {
                googleChatResponse.response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, googleChatResponse.response);
                return googleChatResponse;
            }
            if (conversationChatResponse.matchConfidence < urbanDictionaryThreshold && urbanDictionaryChatResponse.confidence > conversationChatResponse.matchConfidence)
            {
                urbanDictionaryChatResponse.response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, urbanDictionaryChatResponse.response);
                return urbanDictionaryChatResponse;
            }

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var response = _responseConversionService.ConvertResponse(analyzedConversation.responses.Last(), matchChat);

            //TODO: alter reply to match sophistication

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, response)
            };

            return chatResponse;
        }

        public ChatResponse GetResponse(Conversation analyzedConversation, List<string> requiredTypes, List<string> requiredProperyMatches, List<string> excludedTypes, List<string> subjectGoals)
        {
            //TODO: change type to list of types, pass that in. if it's empty do any
            var conversationChatResponse = _conversationMatchService.GetConversationMatch(analyzedConversation, requiredTypes, requiredProperyMatches, excludedTypes, subjectGoals);
            var userPropertyChatResponse = _userPropertyMatchService.GetUserPropertyMatch(analyzedConversation);

            var matchChat = conversationChatResponse;

            if (userPropertyChatResponse.confidence > conversationChatResponse.matchConfidence) //TODO: check the uniqueness of reply (if it was already used)
            {
                userPropertyChatResponse.response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, userPropertyChatResponse.response);
                return userPropertyChatResponse;
            }

            if (matchChat.responseChat == null)
            {
                return new ChatResponse { confidence = 0, response = new List<string>() };
            }

            var response = _responseConversionService.ConvertResponse(analyzedConversation.responses.Last(), matchChat);

            var chatResponse = new ChatResponse
            {
                confidence = matchChat.matchConfidence,
                response = _salutationService.GetProperlyAddressedResponse(analyzedConversation, response)
            };

            //TODO: alter reply to match sophistication

            return chatResponse;
        }
    }
}
