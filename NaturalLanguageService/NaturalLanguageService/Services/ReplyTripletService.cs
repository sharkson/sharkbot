using ChatModels;
using System.Collections.Generic;

namespace NaturalLanguageService.Services
{
    public class ReplyTripletService
    {
        public Triplets GetReplyTriplets(Sentence sentence)
        {
            var triplets = new Triplets();
            //TODO: other patterns
            if (IsSubjectPredicateObjectPattern(sentence.chunks))
            {
                triplets = GetSubjectPredicateObjectTriplets(sentence.chunks);

            }
            else if (IsSubjectObjectPattern(sentence.chunks))
            {
                triplets = GetSubjectObjectTriplets(sentence.chunks);
            }
            return triplets;
        }

        private bool IsSubjectPredicateObjectPattern(List<Chunk> chunks)
        {
            return chunks.FindIndex(c => c.tag == "NP") < chunks.FindIndex(c => c.tag == "VP") && chunks.FindIndex(c => c.tag == "VP") < chunks.FindLastIndex(c => c.tag == "NP");
        }

        private bool IsSubjectObjectPattern(List<Chunk> chunks)
        {
            return chunks.FindIndex(c => c.tag == "NP") < chunks.FindIndex(c => c.tag == "VP");
        }

        private Triplets GetSubjectPredicateObjectTriplets(List<Chunk> chunks)
        {
            var triplets = new Triplets();
            var subject = chunks.Find(c => c.tag == "NP");
            if (subject != null)
            {
                triplets.subject = new Subject() { chunk = subject, confidence = 1 };
            }

            var predicate = chunks.Find(c => c.tag == "VP");
            if (predicate != null)
            {
                triplets.predicate = new Predicate() { chunk = predicate, confidence = 1 };
            }

            var objectChunk = chunks.FindLast(c => c.tag == "NP");
            if (objectChunk != null)
            {
                triplets.objectTriplet = new ObjectTriplet() { chunk = objectChunk, confidence = 1 };
            }
            return triplets;
        }

        private Triplets GetSubjectObjectTriplets(List<Chunk> chunks)
        {
            var triplets = new Triplets();
            var subject = chunks.Find(c => c.tag == "NP");
            if (subject != null)
            {
                triplets.subject = new Subject() { chunk = subject, confidence = 1 };
            }

            var objectChunk = chunks.Find(c => c.tag == "VP");
            if (objectChunk != null)
            {
                triplets.objectTriplet = new ObjectTriplet() { chunk = objectChunk, confidence = 1 };
            }
            return triplets;
        }
    }
}
