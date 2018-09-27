using ChatModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserService
{
    public class PropertyFromQuestionService
    {
        public UserProperty getPropertyFromQuestion(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            var property = new UserProperty();

            if (question == null)
            {
                return property;
            }

            property = getSelfPropertyFromQuestion(analyzedChat, question);

            if(string.IsNullOrEmpty(property.name))
            {
                property = getAnyPropertyFromQuestion(analyzedChat, question);
            }

            return property;
        }

        private List<string> selfPropertyQuestion = new List<string>() { "are you an (\\p{L}*)", "are you a (\\p{L}*)", "are you (\\p{L}*)" }; //TODO: account for multiple words after are you, ex. are you a big fish? //TODO: are you an X or a Y, answer X
        private List<string> yesAnswers = new List<string>() { "(\\byes\\b)", "(\\byea\\b)", "(\\byeah\\b)", "(\\bya\\b)", "(\\byep\\b)", "(\\byup\\b)", "(\\byas\\b)", "(\\bmhm\\b)", "(\\byou know it\\b)", "(\\baffirmative\\b)", "(\\buh-huh\\b)" };
        private List<string> noAnswers = new List<string>() { "(\\bno\\b)", "(\\bnope\\b)", "(\\bnah\\b)", "(\\bnegative\\b)", "(\\buh-uh\\b)" };
        private UserProperty getSelfPropertyFromQuestion(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            var property = new UserProperty();

            foreach (var regex in selfPropertyQuestion)
            {
                var match = getPropertyQuestionMatch(question.chat.message, regex);
                if (!string.IsNullOrWhiteSpace(match))
                {
                    bool yes = false;
                    bool no = false;
                    if (yesAnswers.Any(y => responseMatch(analyzedChat.chat.message, y)))
                    {
                        yes = true;
                        property.name = "self";
                        property.value = match;
                        property.source = analyzedChat.chat.user;
                        property.time = analyzedChat.chat.time;
                    }
                    if (noAnswers.Any(n => responseMatch(analyzedChat.chat.message, n)))
                    {
                        no = true;
                        property.name = "self";
                        property.value = "not " + match;
                        property.source = analyzedChat.chat.user;
                        property.time = analyzedChat.chat.time;
                    }

                    if (yes && no)
                    {
                        return new UserProperty();
                    }

                    if ((yes && !no) || (no && !yes))
                    {
                        return property;
                    }
                }
            }

            return property;
        }

        private UserProperty getAnyPropertyFromQuestion(AnalyzedChat analyzedChat, AnalyzedChat question)
        {
            return new UserProperty(); //TODO: do you have XXX? (do you have hair, legs, etc.) do you have XXX XXX?  (green eyes, red hair) etc.
        }

        private string getPropertyQuestionMatch(string source, string regex)
        {
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 2)
            {
                var badMatch = Regex.Match(source, regex + " (\\p{L}*)", RegexOptions.IgnoreCase);
                if(badMatch.Groups.Count < 3)
                {
                    return match.Groups[1].Value;
                }
            }
            return string.Empty;
        }

        private bool responseMatch(string source, string regex)
        {
            var match = Regex.Match(source, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 2)
            {
                return true;
            }

            return false;
        }
    }
}
