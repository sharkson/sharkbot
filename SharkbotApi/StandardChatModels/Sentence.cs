using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class Sentence
    {
        public List<Token> tokens { get; set; }

        public List<Chunk> chunks { get; set; }

        public Triplets triplets { get; set; }

        public bool interrogative { get; set; }
    }
}