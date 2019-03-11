using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace StanfordNaturalLanguageService
{
    public class TokenService
    {
        public List<Token> GetTokens(SimpleNetNlp.Sentence stanfordSentence)
        {
            var tokens = new List<Token>();
            for (int i = 0; i < stanfordSentence.Words.Count; i++)
            {
                var token = new Token();
                token.Word = stanfordSentence.Words.ElementAt(i);
                token.Lemmas = stanfordSentence.Lemmas.ElementAt(i);
                token.PosTag = stanfordSentence.PosTags.ElementAt(i);
                token.NerTag = stanfordSentence.NerTags.ElementAt(i);
                token.IncomingDependencyLabel = stanfordSentence.IncomingDependencyLabels.ElementAt(i);
                token.Governor = stanfordSentence.Governors.ElementAt(i);
                tokens.Add(token);
            }
            return tokens;
        }
    }
}
