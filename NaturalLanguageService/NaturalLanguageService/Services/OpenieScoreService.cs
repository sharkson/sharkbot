using System;
using System.Collections.Generic;
using ChatModels;

namespace NaturalLanguageService.Services
{
    public class OpenieScoreService
    {
        public double GetOpenieScore(List<OpenieTriple> target, List<OpenieTriple> existing)
        {
            var bestScore = 0;
            foreach(var targetOpenie in target)
            {
                foreach(var existingOpenie in existing)
                {
                    var score = 0;
                    if(targetOpenie.Subject.ToLower() == existingOpenie.Subject.ToLower())
                    {
                        score++;
                    }
                    if (targetOpenie.Relation.ToLower() == existingOpenie.Relation.ToLower())
                    {
                        score++;
                    }
                    if (targetOpenie.Object.ToLower() == existingOpenie.Object.ToLower())
                    {
                        score++;
                    }
                    if(score > bestScore)
                    {
                        bestScore = score;
                    }
                }
            }
            return bestScore/3;
        }
    }
}