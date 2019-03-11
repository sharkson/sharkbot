using ChatModels;

namespace NaturalLanguageService.Services
{
    public class VoiceScoreService
    {
        public double GetScore(Voice target, Voice existing)
        {
            if(target == existing)
            {
                return 1.0;
            }

            if(target == Voice.Unidentifiable || existing == Voice.Unidentifiable)
            {
                return 0.25;
            }

            return 0.0;
        }
    }
}
