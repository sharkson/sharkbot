using System;

namespace ChatModels
{
    [Serializable]
    public class Triplet
    {
        public double confidence { get; set; }
        public Chunk chunk { get; set; }
    }
}