using ChatModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class UserNickNameService
    {
        private List<string> nameSearch = new List<string>() { "call me (\\p{L}*)", "my name is (\\p{L}*)" }; //I am XXX, but make sure it's a name and not an adjective like hungry, good, etc., or I am a big guy, etc.
        private List<string> excludeNameSearch = new List<string>() { "don't call me", "not call me", "never call me", " is not " };
        //TODO: determine if someone else is calling someone by a nickname.  If there is a proper noun and it is similar to their username, for example username sharknice and being called sharkie  4 consecutive letters match
        public string GetNickName(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            var nickName = getNickNameFromResponse(analyzedChat);

            if(nickName == string.Empty)
            {
                nickName = getNickNameFromQuestion(analyzedChat, question);
            }

            return nickName;
        }

        private string getNickNameFromResponse(AnalyzedChat analyzedChat)
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

        private string getNickNameFromQuestion(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            if(question == null || question.naturalLanguageData.responseConfidence < .75 || !isAskingForName(question))
            {
                return string.Empty;
            }

            var nickName = analyzedChat.naturalLanguageData.userlessMessage.Trim();
            if (!nickName.Contains(" ") && analyzedChat.naturalLanguageData.sentences.Count > 0 && !analyzedChat.naturalLanguageData.sentences[0].interrogative)
            {
                if(isNaturalLanguageName(analyzedChat, nickName))
                {
                    return nickName;
                }
            }

            return string.Empty;
        }

        private List<string> questionSearch = new List<string>() { "what's your name", "what is your name", "what should I call you", "who are you" };
        private bool isAskingForName(AnalyzedChat question)
        {
            return questionSearch.Any(q => question.chat.message.ToLower().Contains(q));
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
