using ChatModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class UserNickNameService
    {
        private List<string> nameSearch = new List<string>() { "call me (\\p{L}*)", "my name is (\\p{L}*)" };
        private List<string> excludeNameSearch = new List<string>() { "don't call me", "not call me", "never call me", " is not " };

        public string GetNickName(AnalyzedChat analyzedChat)
        {
            if (!excludeNameSearch.Any(e => analyzedChat.chat.message.ToLower().Contains(e)))
            {
                foreach (var regex in nameSearch)
                {
                    var match = getNameMatch(analyzedChat.chat.message, regex);
                    if (!string.IsNullOrWhiteSpace(match) && isNaturalLanguageName(analyzedChat, match))
                    {
                        return match;
                    }
                }
            }
            return string.Empty;
        }

        private string getNameMatch(string source, string regex)
        {
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count > 0)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        private bool isNaturalLanguageName(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.Lexeme == match)
                    {
                        return token.POSTag == "NN" || token.POSTag == "NNP" || token.POSTag == "VBG";
                    }
                }
            }
            return false;
        }
    }
}
