using ChatModels;
using NaturalLanguageService.Services;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class VoiceScoreService_GetVoice
    {
        [Fact]
        public void GetVoice()
        {
            var service = new VoiceScoreService();

            var result = service.GetScore(Voice.Active, Voice.Active);
            Assert.Equal(1.0, result);
            result = service.GetScore(Voice.Passive, Voice.Passive);
            Assert.Equal(1.0, result);
            result = service.GetScore(Voice.Unidentifiable, Voice.Unidentifiable);
            Assert.Equal(1.0, result);

            result = service.GetScore(Voice.Passive, Voice.Active);
            Assert.Equal(0.0, result);
            result = service.GetScore(Voice.Active, Voice.Passive);
            Assert.Equal(0.0, result);

            result = service.GetScore(Voice.Active, Voice.Unidentifiable);
            Assert.Equal(0.25, result);
            result = service.GetScore(Voice.Unidentifiable, Voice.Passive);
            Assert.Equal(0.25, result);
        }
    }
}
