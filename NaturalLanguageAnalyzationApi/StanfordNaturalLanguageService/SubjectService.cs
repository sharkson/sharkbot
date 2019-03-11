using System.Collections.Generic;
using System.Linq;
using ChatModels;

namespace StanfordNaturalLanguageService
{
    public class SubjectService
    {
        public Token GetSubject(List<OpenieTriple> openieTriples, List<Token> tokens, SentenceType sentenceType)
        {
            Token subject;
            if (openieTriples.Count > 0)
            {
                subject = tokens.FirstOrDefault(t => t.Word == openieTriples.First().Subject);
                if (subject != null)
                {
                    return subject;
                }
            }

            subject = tokens.FirstOrDefault(t => t.IncomingDependencyLabel != null && (t.IncomingDependencyLabel.StartsWith("nsubj") || t.IncomingDependencyLabel.StartsWith("csubj")));
            if(subject != null)
            {
                return subject;
            }

            if(sentenceType == SentenceType.Imperative)
            {
                return new Token { Word = "you", Lemmas = "you" };
            }

            return new Token();
        }
    }
}
