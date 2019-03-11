using System;

namespace ChatModels
{
    [Serializable]
    public class ReadingLevel
    {
        public double FleschKincaidReadingEase { get; set; }

        public double FleschKincaidGradeLevel { get; set; }

        public double GunningFogScore { get; set; }

        public double ColemanLiauIndex { get; set; }

        public double SMOGIndex { get; set; }

        public double AutomatedReadabilityIndex { get; set; }
    }
}