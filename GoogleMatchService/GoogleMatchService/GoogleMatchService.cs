﻿using ChatModels;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GoogleMatchService
{
    public class GoogleMatchService
    {
        private readonly ScrapingBrowser _browser;
        private const string searchUrl = "http://www.google.com/search?q=";
        private const double googleConfidence = .9;

        public GoogleMatchService(ScrapingBrowser scrapingBrowser)
        {
            _browser = scrapingBrowser;
            ignoreList = new List<string> { "how are you", "what is that", "how is it going" };
        }

        public ChatResponse GetGoogleMatch(Conversation conversation)
        {
            var request = conversation.responses.Last();
            var searchText = request.chat.message.Replace("@" + request.chat.botName, string.Empty).Replace(request.chat.botName, string.Empty).Trim();

            var response = new List<string>();

            var googleResult = GetSearchResult(searchText);
            if (!string.IsNullOrWhiteSpace(googleResult))
            {
                response.Add(googleResult);
                return new ChatResponse { confidence = googleConfidence, response = response };
            }

            return new ChatResponse { confidence = 0, response = response };
        }

        private List<string> ignoreList;

        private string GetSearchResult(string searchString)
        {
            try
            {
                var searchResult = _browser.NavigateToPage(new Uri(searchUrl + searchString));

                if (DoTextSearch(searchString))
                {
                    var definition = CleanResult(GetDefinition(searchResult));
                    if (!string.IsNullOrWhiteSpace(definition))
                    {
                        return definition;
                    }

                    var time = CleanResult(GetTime(searchResult));
                    if (!string.IsNullOrWhiteSpace(time))
                    {
                        return time;
                    }

                    var death = CleanResult(GetDeath(searchResult));
                    if (!string.IsNullOrWhiteSpace(death))
                    {
                        return death;
                    }

                    var longDescription = CleanResult(GetLongDescription(searchResult));
                    if (!string.IsNullOrWhiteSpace(longDescription))
                    {
                        return longDescription;
                    }

                    var translation = CleanResult(GetTranslation(searchResult));
                    if (!string.IsNullOrWhiteSpace(translation))
                    {
                        return translation;
                    }

                    var wiki = CleanResult(GetWikipediaSummary(searchResult));
                    if (!string.IsNullOrWhiteSpace(wiki))
                    {
                        return wiki;
                    }
                }

                var calculation = CleanResult(GetCalculation(searchResult));
                if (!string.IsNullOrWhiteSpace(calculation))
                {
                    return calculation;
                }
            }
            catch (AggregateException exception)
            {
                Console.WriteLine(exception);
            }
            return string.Empty;
        }

        private bool DoTextSearch(string searchString)
        {
            return searchString.Contains(" ") && searchString.Length > 15 && !ignoreList.Any(i => searchString.ToLower().Contains(i));
        }

        private string CleanResult(string result)
        {
            return WebUtility.HtmlDecode(result).Replace("???", string.Empty).Replace("??", " ");
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

        private string GetCalculation(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect("#cwos");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        private string GetTranslation(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect("#tw-target-text");
            if (answer.Count() > 0 && !string.IsNullOrWhiteSpace(answer.First().InnerText))
            {
                return answer.First().InnerText;
            }
            return string.Empty;
        }

        private string GetWikipediaSummary(WebPage searchResult)
        {
            var answer = searchResult.Html.CssSelect(".ILfuVd");
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
