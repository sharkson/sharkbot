using ChatModels;
using System.Collections.Generic;

namespace ChatAnalyzer.Services
{
    public class ConversationReadingLevelService
    {
        public ReadingLevel GetReadingLevel(List<AnalyzedChat> responses)
        {
            var conversationText = GetConversationText(responses);
            var textStatistics = TextStatistics.Net.TextStatistics.Parse(conversationText);

            var readingLevel = new ReadingLevel();

            readingLevel.FleschKincaidReadingEase = textStatistics.FleschKincaidReadingEase();
            readingLevel.FleschKincaidGradeLevel = textStatistics.FleschKincaidGradeLevel();
            readingLevel.GunningFogScore = textStatistics.GunningFogScore();
            readingLevel.ColemanLiauIndex = textStatistics.ColemanLiauIndex();
            readingLevel.SMOGIndex = textStatistics.SMOGIndex();
            readingLevel.AutomatedReadabilityIndex = textStatistics.AutomatedReadabilityIndex();

            return readingLevel;
        }

        private string GetConversationText(List<AnalyzedChat> responses)
        {
            var conversationText = string.Empty;

            foreach(var response in responses)
            {
                conversationText += response.chat.message + ".";
            }

            return conversationText;
        }
    }
}
