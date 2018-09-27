using ChatModels;

namespace UserService
{
    public class UserNaturalLanguageService
    {
        public bool isNaturalLanguagePropertyName(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.Lexeme == match)
                    {
                        return token.POSTag == "NN" || token.POSTag == "NNP" || token.POSTag == "NNS";
                    }
                }
            }
            return false;
        }

        public bool isNaturalLanguageSelfProperty(AnalyzedChat chat, string match)
        {
            return true; //TODO: probably adjectives
        }

        public bool isNaturalLanguagePropertyValue(AnalyzedChat chat, string match)
        {
            foreach (var sentence in chat.naturalLanguageData.sentences)
            {
                foreach (var token in sentence.tokens)
                {
                    if (token.Lexeme == match)
                    {
                        return token.POSTag == "JJ";
                    }
                }
            }
            return false;
        }
    }
}
