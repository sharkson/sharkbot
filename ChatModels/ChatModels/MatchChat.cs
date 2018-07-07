using System;

namespace ChatModels
{
    [Serializable]
    public class MatchChat
    {
        public AnalyzedChat analyzedChat { get; set; }
        public AnalyzedChat responseChat { get; set; }
        public double matchConfidence { get; set; }
    }
}
