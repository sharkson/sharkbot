using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace StanfordNaturalLanguageService
{
    public class ExclamatoryService
    {
        public bool IsExclamatory(List<Token> tokens)
        {
            if (tokens.Any(t => t.Word == "?"))
            {
                return false;
            }

            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VB") && tokens.FindIndex(t => t.PosTag == "VB") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBD") && tokens.FindIndex(t => t.PosTag == "VBD") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBP") && tokens.FindIndex(t => t.PosTag == "VBP") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "PRP") < tokens.FindIndex(t => t.PosTag == "VBZ") && tokens.FindIndex(t => t.PosTag == "VBZ") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VB") && tokens.FindIndex(t => t.PosTag == "VB") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBD") && tokens.FindIndex(t => t.PosTag == "VBD") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBP") && tokens.FindIndex(t => t.PosTag == "VBP") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.PosTag == "VBZ") && tokens.FindIndex(t => t.PosTag == "VBZ") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VB") && tokens.FindIndex(t => t.PosTag == "VB") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBD") && tokens.FindIndex(t => t.PosTag == "VBD") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBP") && tokens.FindIndex(t => t.PosTag == "VBP") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNP") < tokens.FindIndex(t => t.PosTag == "VBZ") && tokens.FindIndex(t => t.PosTag == "VBZ") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VB") && tokens.FindIndex(t => t.PosTag == "VB") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBD") && tokens.FindIndex(t => t.PosTag == "VBD") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBP") && tokens.FindIndex(t => t.PosTag == "VBP") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "NNS") < tokens.FindIndex(t => t.PosTag == "VBZ") && tokens.FindIndex(t => t.PosTag == "VBZ") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }

            if (tokens.FindIndex(t => t.PosTag == "JJ") < tokens.FindIndex(t => t.PosTag == "NN") && tokens.FindIndex(t => t.PosTag == "NN") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            if (tokens.FindIndex(t => t.PosTag == "JJ") < tokens.FindIndex(t => t.PosTag == "VBG") && tokens.FindIndex(t => t.PosTag == "VBG") < tokens.FindIndex(t => t.Word == "!"))
            {
                return true;
            }
            //WDT JJ NN, JJ NN
            return false;
        }
    }
}
