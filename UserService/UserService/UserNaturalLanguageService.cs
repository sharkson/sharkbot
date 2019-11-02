using ChatModels;
using System.Linq;

namespace UserService
{
    public class UserNaturalLanguageService
    {
        public bool isNaturalLanguagePropertyName(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.Tokens)
                {
                    if (token.Word == match)
                    {
                        return token.NerTag == "PERSON" || token.PosTag == "NN" || token.PosTag == "NNP" || token.PosTag == "NNS";
                    }
                }
            }
            return false;
        }

        public bool isNaturalLanguageSelfProperty(AnalyzedChat chat, string match)
        {
            if (chat.naturalLanguageData.sentences == null)
            {
                return false;
            }

            var tokens = chat.naturalLanguageData.sentences.SelectMany(s => s.Tokens);
            var token = tokens.FirstOrDefault(t => t.Word.ToLower() == match.ToLower());
            if(token != null && token.PosTag.StartsWith("VB"))
            {
                return false;
            }

            return true;
        }

        public bool isNaturalLanguagePropertyValue(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.Tokens)
                {
                    if (token.Word == match)
                    {
                        return token.PosTag == "JJ";
                    }
                }
            }
            return false;
        }
    }
}
