using ChatModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SharkbotReplier.Services
{
    public class UrbanDictionaryMatchService
    {
        private const double urbanDictionaryMatchScore = .9;
        private const double urbanDictionaryTokenScore = .25;

        public ChatResponse GetUrbanDictionaryMatch(Conversation analyzedConversation)
        {
            var request = analyzedConversation.responses.Last();

            var matchedDefinition = GetDefinitionFormatMatch(request);
            if (!string.IsNullOrWhiteSpace(matchedDefinition))
            {
                return new ChatResponse { confidence = urbanDictionaryMatchScore, response = matchedDefinition };
            }

            if (request.naturalLanguageData.sentences[0].triplets.objectTriplet != null)
            {
                var definitions = new List<ChatResponse>();
                foreach (var targetToken in request.naturalLanguageData.sentences[0].triplets.objectTriplet.chunk.tokens)
                {
                    if (targetToken.POSTag == "NN" || targetToken.POSTag == "NNS" || targetToken.POSTag == "NNP" || targetToken.POSTag == "NNPS" || targetToken.POSTag == "VBG")
                    {
                        var definition = GetDefinition(targetToken.Lexeme);
                        if (definition.confidence > 0)
                        {
                            definitions.Add(definition);
                        }
                    }
                }

                if (definitions.Count() > 0)
                {
                    var bestDefinition = definitions.OrderByDescending(x => x.confidence).First();
                    bestDefinition.confidence = urbanDictionaryTokenScore;
                    return bestDefinition;
                }
            }

            return new ChatResponse() { response = string.Empty, confidence = 0 };
        }

        private int scoreThreshold = 5;

        private List<string> definitionSearches = new List<string>() { "what does (.*) mean", "what's (.*) mean", "what is a (.*)", "what are (.*)", "what's a (.*)", "what's (.*)", "what is a (.*) ", "what are (.*) ", "what's a (.*) ", "what is a (.*)\\?", "what's a (.*)\\?", "what's (.*)\\?", "what are (.*)\\?", "what is (.*)", "what is (.*)\\?" };
        private List<string> excludedWords = new List<string>() { "it", "that", "they" };

        public ChatResponse GetDefinition(string word)
        {
            if (excludedWords.Any(e => word.Contains(e)))
            {
                return new ChatResponse() { response = string.Empty, confidence = 0 };
            }

            dynamic result = string.Empty;
            using (var webClient = new WebClient())
            {
                try
                {
                    result = webClient.DownloadString("http://api.urbandictionary.com/v0/define?term=" + word);
                }
                catch (Exception)
                {

                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = JObject.Parse(result);
                if (result.list.Count > 0)
                {
                    var bestResult = ((IEnumerable)result.list).Cast<dynamic>().OrderByDescending(e => e.thumbs_up - e.thumbs_down).FirstOrDefault();
                    var score = bestResult.thumbs_up - bestResult.thumbs_down;
                    if (score > scoreThreshold)
                    {
                        return new ChatResponse() { response = bestResult.definition, confidence = score };
                    }
                }
            }

            return new ChatResponse() { response = string.Empty, confidence = 0 };
        }

        private string GetDefinitionFormatMatch(AnalyzedChat chat)
        {
            var searchText = chat.chat.message;
            if (searchText.Contains(chat.botName))
            {
                searchText = searchText.Replace("@" + chat.botName, string.Empty).Replace(chat.botName, string.Empty);
            }

            if (excludedWords.Any(e => searchText.Contains(e)))
            {
                return string.Empty;
            }

            foreach (var regex in definitionSearches)
            {
                var word = getMatch(searchText, regex);
                if (word != string.Empty)
                {
                    dynamic result = string.Empty;
                    using (var webClient = new WebClient())
                    {
                        try
                        {
                            result = webClient.DownloadString("http://api.urbandictionary.com/v0/define?term=" + word);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        result = JObject.Parse(result);
                        if (result.list.Count > 0)
                        {
                            var bestResult = ((IEnumerable)result.list).Cast<dynamic>().OrderByDescending(e => e.thumbs_up - e.thumbs_down).FirstOrDefault();
                            var score = bestResult.thumbs_up - bestResult.thumbs_down;
                            if (score > scoreThreshold)
                            {
                                return formatDefinition((string)bestResult.definition);
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        private string formatDefinition(string definition)
        {
            return definition.Replace("[", "").Replace("]", "");
        }

        private string getMatch(string source, string regex)
        {
            var match = Regex.Match(source.ToLower(), regex);
            if (match.Groups.Count > 0)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}