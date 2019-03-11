using System;

namespace ChatModels
{
    [Serializable]
    public class Token
    {
        public string POSTag { get; set; }

        public string Lexeme { get; set; }

        public string Stem { get; set; }
    }
}