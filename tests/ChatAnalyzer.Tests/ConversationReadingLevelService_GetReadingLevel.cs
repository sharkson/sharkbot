using ChatAnalyzer.Services;
using ChatModels;
using System.Collections.Generic;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class ConversationReadingLevelService_GetReadingLevel
    {
        [Fact]
        public void GetReadingLevel()
        {
            var service = new ConversationReadingLevelService();
            var responses = new List<AnalyzedChat>();
            var analyzedChat = new AnalyzedChat
            {
                botName = "sharkbot",
                chat = new Chat
                {
                    message = "suh",
                    user = "tester",
                    botName = "sharkbot"
                }
            };
            responses.Add(analyzedChat);
            var result = service.GetReadingLevel(responses);
            Assert.Equal(-6.8, result.AutomatedReadabilityIndex);
            Assert.Equal(1.6, result.ColemanLiauIndex);
            Assert.Equal(-3.4, result.FleschKincaidGradeLevel);
            Assert.Equal(121.2, result.FleschKincaidReadingEase);
            Assert.Equal(0.4, result.GunningFogScore);
            Assert.Equal(1.8, result.SMOGIndex);
        }
    }
}
