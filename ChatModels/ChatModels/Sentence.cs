using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class Sentence
    {
        public string Source { get; set; }
        public List<Token> Tokens { get; set; }
        public double Sentiment { get; set; }
        public SentenceType SentenceType { get; set; }
        public Voice Voice { get; set; }
        public Token Subject { get; set; }
        public Token Predicate { get; set; }
        public Token Object { get; set; }
        public List<OpenieTriple> OpenieTriples { get; set; }
    }

    public enum SentenceType { Unidentifiable, Declarative, Interrogative, Imperative, Exclamatory };
    public enum Voice { Unidentifiable, Active, Passive };
}