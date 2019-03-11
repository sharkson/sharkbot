using System.Collections.Generic;
using System.Linq;
using ChatModels;

namespace StanfordNaturalLanguageService
{
    public class VoiceService
    {
        public Voice GetVoice(List<Token> tokens)
        {
            if(tokens.Any(t => t.IncomingDependencyLabel != null && (t.IncomingDependencyLabel == "nsubj" || t.IncomingDependencyLabel.StartsWith("nsubj:") || t.IncomingDependencyLabel == "csubj" || t.IncomingDependencyLabel.StartsWith("csubj:"))))
            {
                return Voice.Active;
            }
            if (tokens.Any(t => t.IncomingDependencyLabel != null && t.IncomingDependencyLabel.StartsWith("nsubjpass")))
            {
                return Voice.Passive;
            }
            return Voice.Unidentifiable;
        }
    }
}
