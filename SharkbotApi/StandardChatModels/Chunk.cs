using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class Chunk
    {
        public string tag { get; set; }
        public List<Token> tokens { get; set; }
    }
}