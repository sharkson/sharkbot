using ChatModels;
using System;

namespace ConversationMatcher.Services
{
    public class ReadingLevelConfidenceService
    {       
        public double GetReadingLevelConfidence(ReadingLevel targetReadingLevel, ReadingLevel existingReadingLevel)
        {
            var fleschKincaidReadingEaseScore = getMatchConfidence(targetReadingLevel.FleschKincaidReadingEase, existingReadingLevel.FleschKincaidReadingEase, 100.0);
            var fleschKincaidGradeLevelScore = getMatchConfidence(targetReadingLevel.FleschKincaidGradeLevel, existingReadingLevel.FleschKincaidGradeLevel, 12.0);
            var gunningFogScore = getMatchConfidence(targetReadingLevel.GunningFogScore, existingReadingLevel.GunningFogScore, 18.0);
            var colemanLiauIndex = getMatchConfidence(targetReadingLevel.ColemanLiauIndex, existingReadingLevel.ColemanLiauIndex, 18.0);
            var sMOGIndex = getMatchConfidence(targetReadingLevel.SMOGIndex, existingReadingLevel.SMOGIndex, 18.0);
            var automatedReadabilityIndex = getMatchConfidence(targetReadingLevel.AutomatedReadabilityIndex, existingReadingLevel.AutomatedReadabilityIndex, 14.0);

            var sum = fleschKincaidReadingEaseScore + fleschKincaidGradeLevelScore + gunningFogScore + colemanLiauIndex + sMOGIndex + automatedReadabilityIndex;
            return sum / 6.0;
        }

        private double getMatchConfidence(double target, double existing, double maxDifference)
        {
            var difference = Math.Abs(target - existing);
            if (difference > maxDifference)
            {
                difference = maxDifference;
            }

            return 1.0 - (difference / maxDifference);
        }
    }
}
