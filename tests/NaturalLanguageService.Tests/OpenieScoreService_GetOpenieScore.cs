using ChatModels;
using NaturalLanguageService.Services;
using System.Collections.Generic;
using Xunit;

namespace NaturalLanguageService.Tests
{
    public class OpenieScoreService_GetOpenieScore
    {
        [Fact]
        public void PerfectMatch()
        {
            var service = new OpenieScoreService();

            var targetTriples = new List<OpenieTriple>();
            targetTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var target = new Sentence { OpenieTriples = targetTriples };

            var existingTriples = new List<OpenieTriple>();
            existingTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var existing = new Sentence { OpenieTriples = existingTriples };

            var result = service.GetOpenieScore(targetTriples, existingTriples);

            Assert.Equal(1, result);
        }

        [Fact]
        public void TwoMatch()
        {
            var service = new OpenieScoreService();

            var targetTriples = new List<OpenieTriple>();
            targetTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "pizza" });
            var target = new Sentence { OpenieTriples = targetTriples };

            var existingTriples = new List<OpenieTriple>();
            existingTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var existing = new Sentence { OpenieTriples = existingTriples };

            var result = service.GetOpenieScore(targetTriples, existingTriples);

            Assert.Equal(2/3, result);
        }

        [Fact]
        public void OneMatch()
        {
            var service = new OpenieScoreService();

            var targetTriples = new List<OpenieTriple>();
            targetTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "like", Object = "pizza" });
            var target = new Sentence { OpenieTriples = targetTriples };

            var existingTriples = new List<OpenieTriple>();
            existingTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var existing = new Sentence { OpenieTriples = existingTriples };

            var result = service.GetOpenieScore(targetTriples, existingTriples);

            Assert.Equal(2 / 3, result);
        }

        [Fact]
        public void NoMatch()
        {
            var service = new OpenieScoreService();

            var targetTriples = new List<OpenieTriple>();
            targetTriples.Add(new OpenieTriple { Subject = "dolphins", Relation = "like", Object = "pizza" });
            var target = new Sentence { OpenieTriples = targetTriples };

            var existingTriples = new List<OpenieTriple>();
            existingTriples.Add(new OpenieTriple { Subject = "sharks", Relation = "eat", Object = "meat" });
            var existing = new Sentence { OpenieTriples = existingTriples };

            var result = service.GetOpenieScore(targetTriples, existingTriples);

            Assert.Equal(2 / 3, result);
        }
    }
}
