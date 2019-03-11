using System.Collections.Generic;
using System.Linq;
using ChatModels;

namespace StanfordNaturalLanguageService
{
    public class PredicateService
    {
        public Token GetPredicate(List<OpenieTriple> openieTriples, List<Token> tokens, SentenceType sentenceType)
        {
            Token predicate;
            if (openieTriples.Count > 0)
            {
                predicate = tokens.FirstOrDefault(t => t.Word == openieTriples.First().Relation);
                if(predicate != null)
                {
                    return predicate;
                }
            }

            predicate = tokens.FirstOrDefault(t => t.IncomingDependencyLabel != null && t.IncomingDependencyLabel.StartsWith("cc"));
            if (predicate != null)
            {
                return predicate;
            }

            predicate = tokens.FirstOrDefault(t => t.IncomingDependencyLabel != null && t.IncomingDependencyLabel == "root" && t.PosTag.StartsWith("VB"));
            if (predicate != null)
            {
                return predicate;
            }

            if(sentenceType == SentenceType.Imperative)
            {
                predicate = tokens.FirstOrDefault(t => t.PosTag == "JJ" || t.PosTag.StartsWith("VB"));
                if (predicate != null)
                {
                    return predicate;
                }
            }

            return new Token();
        }
    }
}