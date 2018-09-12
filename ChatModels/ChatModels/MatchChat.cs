using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class MatchChat
    {
        public AnalyzedChat analyzedChat { get; set; }
        public List<AnalyzedChat> responseChat { get; set; }
        public double matchConfidence { get; set; }
    }
}
