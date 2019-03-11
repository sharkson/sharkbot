using System.Collections.Generic;
using System.Linq;
using ChatModels;

namespace StanfordNaturalLanguageService
{
    public class ObjectService
    {
        public Token GetObject(List<OpenieTriple> openieTriples, List<Token> tokens, SentenceType sentenceType)
        {
            Token sentenceObject;
            if (openieTriples.Count > 0)
            {
                sentenceObject = tokens.FirstOrDefault(t => t.Word == openieTriples.First().Object);
                if (sentenceObject != null)
                {
                    return sentenceObject;
                }
            }

            sentenceObject = tokens.FirstOrDefault(t => t.IncomingDependencyLabel != null && t.IncomingDependencyLabel.StartsWith("dobj"));
            if (sentenceObject != null)
            {
                return sentenceObject;
            }

            sentenceObject = tokens.LastOrDefault(t => t.IncomingDependencyLabel != null && t.IncomingDependencyLabel == "root" && t.PosTag.StartsWith("NN"));
            if (sentenceObject != null)
            {
                return sentenceObject;
            }

            return new Token();
        }
    }
}