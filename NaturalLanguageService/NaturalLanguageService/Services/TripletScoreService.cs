using ChatModels;
using System.Collections.Generic;

namespace ConversationMatcher.Services
{
    public class TripletScoreService
    {
        TokenScoreService tokenScoreService;

        public TripletScoreService()
        {
            tokenScoreService = new TokenScoreService();
        }

        public double getBestTripletScore(Triplets targetTriplets, List<Sentence> sentences)
        {
            var bestScore = 0.0;
            foreach (var sentence in sentences)
            {
                if (sentence.triplets.subject != null)
                {
                    var tripletScore = getTripletsScore(targetTriplets, sentence.triplets);

                    if (tripletScore > bestScore)
                    {
                        bestScore = tripletScore;
                    }
                }
            }
            return bestScore;
        }

        private double getTripletsScore(Triplets targetTriplets, Triplets triplets)
        {
            var subjectScore = 0.0;
            foreach (var token in targetTriplets.subject.chunk.tokens)
            {
                subjectScore += tokenScoreService.getTokenScore(token, triplets.subject.chunk.tokens);
            }
            var maxSubjectScore = getMaxTripletScore(targetTriplets.subject.chunk.tokens);

            var predicateScore = 0.0;
            var maxPredicateScore = 0.0;
            if (targetTriplets.predicate != null)
            {
                foreach (var token in targetTriplets.predicate.chunk.tokens)
                {
                    if (triplets.predicate != null)
                    {
                        predicateScore += tokenScoreService.getTokenScore(token, triplets.predicate.chunk.tokens);
                    }
                }
                maxPredicateScore = getMaxTripletScore(targetTriplets.predicate.chunk.tokens);
            }

            var objectScore = 0.0;
            foreach (var token in targetTriplets.objectTriplet.chunk.tokens)
            {
                objectScore += tokenScoreService.getTokenScore(token, triplets.objectTriplet.chunk.tokens);
            }
            var maxObjectScore = getMaxTripletScore(targetTriplets.objectTriplet.chunk.tokens);

            return ((subjectScore / maxSubjectScore) + (predicateScore / maxPredicateScore) + (objectScore / maxObjectScore)) / 3.0;
        }

        private double getMaxTripletScore(List<Token> tokens)
        {
            var score = 0.0;
            foreach (var token in tokens)
            {
                score += tokenScoreService.getTokenValue(token);
            }
            return score;
        }
    }
}
