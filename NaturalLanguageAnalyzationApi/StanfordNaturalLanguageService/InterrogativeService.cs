using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace StanfordNaturalLanguageService
{
    public class InterrogativeService
    {
        public bool IsInterrogative(List<Token> tokens)
        {
            if(tokens.Any(t => t.Word == "?"))
            {
                return true;
            }

            if (tokens.Any(t => t.PosTag == "WP" || t.PosTag == "WP$" || t.PosTag == "WRB") && !tokens.Any(t => t.Word == "!"))
            {
                return true;
            }
            return false;
        }
    }
}
