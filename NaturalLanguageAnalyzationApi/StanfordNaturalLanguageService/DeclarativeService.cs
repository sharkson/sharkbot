using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace StanfordNaturalLanguageService
{
    public class DeclarativeService
    {
        public bool IsDeclarative(List<Token> tokens)
        {
            if(tokens.Any(t => t.Word == "!" || t.Word == "?"))
            {
                return false;
            }

            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VB"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBD"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBP"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBZ"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VB"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBD"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBP"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBZ"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VB"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBD"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBP"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBZ"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VB"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBD"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBP"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBZ"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "JJ") < tokens.FindIndex(t => t.PosTag == "NN"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "JJ") < tokens.FindIndex(t => t.PosTag == "VBG"))
            {
                return true;
            }

            //TODO: true for statement or opinion not ending with a ! or a ?
            //"thank you" "screw you" have implied I
            return false;
        }
    }
}
