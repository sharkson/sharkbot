using System;

namespace NaturalLanguageService.Services
{
    public class SentimentScoreService
    {
        public double GetScore(double target, double existing)
        {
            return (2.0 - Math.Abs(target - existing)) / 2.0;
        }
    }
}
