using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace StanfordNaturalLanguageService
{
    public class ImperativeService
    {
        public bool IsImperative(List<Token> tokens)
        {
            if (tokens.Count == 0 || tokens.Any(t => t.Word == "?"))
            {
                return false;
            }

            if (tokens.First().PosTag == "VB" && tokens.FindIndex(t => t.Word == "you") != 1)
            {
                return true;
            }
            if (tokens.First().PosTag == "VBP")
            {
                return true;
            }
            if (tokens.First().PosTag == "JJ" && tokens.FindIndex(t => t.PosTag == "NN") != 1 && tokens.FindIndex(t => t.PosTag == ",") != 1) //maybe JJ followed by something specific
            {
                return true;
            }
            //VB
            //VBP
            return false;
        }
    }
}
