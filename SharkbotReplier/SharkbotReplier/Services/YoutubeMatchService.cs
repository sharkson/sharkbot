using ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkbotReplier.Services
{
    public class YoutubeMatchService
    {
        private const double youtubeConfidence = .9;
        public ChatResponse GetYoutubeMatch(Conversation conversation)
        {
            var request = conversation.responses.Last();
            var searchText = request.chat.message.Replace("@" + request.chat.botName, string.Empty).Replace(request.chat.botName, string.Empty).Trim();

            var response = new List<string>();

            var youtubeComment = GetYoutubeComment(searchText);
            if (!string.IsNullOrWhiteSpace(youtubeComment))
            {
                response.Add(youtubeComment);
                return new ChatResponse { confidence = youtubeConfidence, response = response };
            }

            return new ChatResponse { confidence = 0, response = response };
        }

        private string GetYoutubeId(string message)
        {
            return string.Empty;
            //https://youtu.be/XaWLLEztrIs
            //https://www.youtube.com/watch?v=XaWLLEztrIs
        }

        private string GetYoutubeComment(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
