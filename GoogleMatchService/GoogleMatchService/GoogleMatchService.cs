using ChatModels;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Linq;
using System.Net;

namespace GoogleMatchService
{
    public class GoogleMatchService
    {
        private ScrapingBrowser browser;
        private const string searchUrl = "https://www.google.com/search?q=";
        private const double googleConfidence = .9;

        public GoogleMatchService()
        {
            browser = new ScrapingBrowser();
        }

        public ChatResponse GetGoogleMatch(Conversation conversation)
        {
            var request = conversation.responses.Last();
            var searchText = request.chat.message.Replace("@" + request.chat.botName, string.Empty).Replace(request.chat.botName, string.Empty);

            var googleResult = GetSearchResult(searchText);
            if (!string.IsNullOrWhiteSpace(googleResult))
            {
                return new ChatResponse { confidence = googleConfidence, response = googleResult };
            }

            return new ChatResponse { confidence = 0, response = string.Empty };
        }

        private string GetSearchResult(string searchString)
        {
            try
            {
                var searchResult = browser.NavigateToPage(new Uri(searchUrl + searchString));

                var definition = CleanResult(GetDefinition(searchResult));
                var time = CleanResult(GetTime(searchResult));
                var death = CleanResult(GetDeath(searchResult));
                var longDescription = CleanResult(GetLongDescription(searchResult));

                if (!string.IsNullOrWhiteSpace(definition))
                {
                    return definition;
                }
                else if (!string.IsNullOrWhiteSpace(time))
                {
                    return time;
                }
                else if (!string.IsNullOrWhiteSpace(death))
                {
                    return death;
                }
                else if (!string.IsNullOrWhiteSpace(longDescription))
                {
                    return longDescription;
                }
            }
            catch (AggregateException)
            {
            }
            return string.Empty;
        }

        private string CleanResult(string result)
        {
            return WebUtility.HtmlDecode(result).Replace("???", string.Empty);
        }

        private string GetTime(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect(".vk_bk.dDoNo");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        private string GetDefinition(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect(".PNlCoe.XpoqFe div span");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        private string GetDeath(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect(".Z0LcW");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        private string GetLongDescription(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect(".Y0NH2b.CLPzrc");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        //never do _h4c _rGd vk_h they could say "waht is my ip address", careful about other stuff like that
        //vk_sh vk_bk gives where am I
    }
}
