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
            //TODO: if they say "what is that" get the definition of the previous response's subject
            var request = analyzedConversation.responses.Last();

            var matchedDefinition = GetDefinitionFormatMatch(request);
            if (!string.IsNullOrWhiteSpace(matchedDefinition))
            {
                var response = new List<string>();
                response.Add(matchedDefinition);
                return new ChatResponse { confidence = urbanDictionaryMatchScore, response = response };
            }

            var tripletDefinition = GetSubjectDefinition(request);
            if(tripletDefinition.confidence > 0)
            {
                return tripletDefinition;
            }

            if (analyzedConversation.responses.Count() > 1)
            {
                var inResponseTo = analyzedConversation.responses[analyzedConversation.responses.Count() - 2];
                var definition = GetDefinitionFromQuestion(request, inResponseTo);
                if (definition.confidence > 0)
                {
                    return definition;
                }
            }

            return new ChatResponse() { response = new List<string>(), confidence = 0 };
        }

        private ChatResponse GetSubjectDefinition(AnalyzedChat request)
        {
            if (request.naturalLanguageData.sentences[0].Subject.Lemmas != null)
            {
                var definitions = new List<ChatResponse>();

                var definition = GetDefinition(request.naturalLanguageData.sentences[0].Subject.Lemmas);
                if (definition.confidence > 0)
                {
                    definitions.Add(definition);
                }


                if (definitions.Count() > 0)
                {
                    var bestDefinition = definitions.OrderByDescending(x => x.confidence).First();
                    bestDefinition.confidence = urbanDictionaryTokenScore;
                    return bestDefinition;
                }
            }

            return new ChatResponse() { response = new List<string>(), confidence = 0 };
        }

        private int scoreThreshold = 5;
        private List<string> definitionSearches = new List<string>() { "what does (.*) mean", "what's (.*) mean", "what (.*) means", "what is a (.*)", "what are (.*)", "what's a (.*)", "what's (.*)\\?", "what's (.*)", "what is a (.*) ", "what are (.*) ", "what's a (.*) ", "what is a (.*)\\?", "what's a (.*)\\?", "what are (.*)\\?", "what is (.*)", "what is (.*)\\?", "what (.*) is\\?" };
        private List<string> excludedWords = new List<string>() { "it", "that", "they", "she", "he", "you", "i" };

        public ChatResponse GetDefinition(string word)
        {
            if (excludedWords.Any(e => e.ToLower() == word))
            {
                return new ChatResponse() { response = new List<string>(), confidence = 0 };
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
                        var response = new List<string>();
                        response.Add(formatDefinition((string)bestResult.definition));
                        return new ChatResponse() { response = response, confidence = score };
                    }
                }
            }

            return new ChatResponse() { response = new List<string>(), confidence = 0 };
        }

        private string GetDefinitionFormatMatch(AnalyzedChat chat)
        {
            var searchText = GetSearchText(chat);

            if (excludedWords.Any(e => searchText.Contains(e)))
            {
                return string.Empty;
            }

            return GetDefinitionMatch(searchText);
        }

        private string GetDefinitionMatch(string searchText)
        {
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
                            var bestResult = ((IEnumerable)result.list).Cast<dynamic>().Where(e => ((string)e.word).ToLower() == word.ToLower()).OrderByDescending(e => e.thumbs_up - e.thumbs_down).FirstOrDefault();
                            if (bestResult != null)
                            {
                                var score = bestResult.thumbs_up - bestResult.thumbs_down;
                                if (score > scoreThreshold)
                                {
                                    return formatDefinition((string)bestResult.definition);
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        private ChatResponse GetDefinitionFromQuestion(AnalyzedChat chat, AnalyzedChat previousChat)
        {
            var searchText = GetSearchText(chat);

            foreach (var regex in definitionSearches)
            {
                var word = getMatch(searchText, regex);
                if (word != string.Empty)
                {
                    if(excludedWords.Any(e => word.Contains(e)))
                    {
                        var tripletDefinition = GetSubjectDefinition(previousChat);
                        if (tripletDefinition.confidence > 0)
                        {
                            return tripletDefinition;
                        }
                    }
                }
            }

            return new ChatResponse() { response = new List<string>(), confidence = 0 };
        }

        private string GetSearchText(AnalyzedChat chat)
        {
            var searchText = chat.chat.message;
            if (searchText.Contains(chat.botName))
            {
                searchText = searchText.Replace("@" + chat.botName, string.Empty).Replace(chat.botName, string.Empty);
            }
            return searchText;
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