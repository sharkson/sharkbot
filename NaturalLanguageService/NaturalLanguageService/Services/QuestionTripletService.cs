using ChatModels;
using System.Collections.Generic;

namespace NaturalLanguageService.Services
{
    public class QuestionTripletService
    {
        private List<string> _isQuestionStarters;
        //private List<string> _verbs;
        private List<string> _pronounsAndNouns;

        //TODO: all of these patterns https://learnenglish.britishcouncil.org/en/beginner-grammar/question-forms-subjectobject-questions

        public QuestionTripletService()
        {
            _isQuestionStarters = new List<string> { "is", "was", "are", "were" };
            //_verbs = new List<string> { "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            _pronounsAndNouns = new List<string> { "PRP", "PRP%", "NN", "NNS", "NNP", "NNPS" };
        }

        public Triplets GetQuestionTriplets(Sentence sentence)
        {
            var triplets = new Triplets();

            if (IsQuestionPattern(sentence)) //Is he a teacher?
            {
                triplets = GetIsQuestionTriplets(sentence);
            }

            //Were dinosaurs big?

            return triplets;
        }

        private bool IsQuestionPattern(Sentence sentence)
        {
            var questionStartIndex = sentence.tokens.FindIndex(t => _isQuestionStarters.Contains(t.Lexeme.ToLower()));
            var subjectStartIndex = sentence.tokens.FindIndex(questionStartIndex + 1, t => _pronounsAndNouns.Contains(t.POSTag));
            var objectStartIndex = sentence.tokens.FindIndex(subjectStartIndex + 1, t => _pronounsAndNouns.Contains(t.POSTag));
            return questionStartIndex >= 0 && questionStartIndex < subjectStartIndex && subjectStartIndex < objectStartIndex;
        }

        private Triplets GetIsQuestionTriplets(Sentence sentence)
        {
            var questionStartIndex = sentence.tokens.FindIndex(t => _isQuestionStarters.Contains(t.Lexeme.ToLower()));
            var subjectStartIndex = sentence.tokens.FindIndex(questionStartIndex + 1, t => _pronounsAndNouns.Contains(t.POSTag));
            var objectStartIndex = sentence.tokens.FindIndex(subjectStartIndex + 1, t => _pronounsAndNouns.Contains(t.POSTag));

            var triplets = new Triplets();
            var predicate = sentence.tokens[questionStartIndex];
            if (predicate != null)
            {
                var predicateChunk = new Chunk
                {
                    tokens = new List<Token> { predicate },
                    tag = predicate.POSTag
                };
                triplets.predicate = new Predicate() { chunk = predicateChunk, confidence = 1 };
            }

            var subject = sentence.tokens[subjectStartIndex];
            if (subject != null)
            {
                var subjectChunk = new Chunk
                {
                    tokens = new List<Token> { subject },
                    tag = subject.POSTag
                };
                triplets.subject = new Subject() { chunk = subjectChunk, confidence = 1 };
            }

            var objectToken = sentence.tokens[objectStartIndex];
            if (objectToken != null)
            {
                var objectChunk = new Chunk
                {
                    tokens = new List<Token> { objectToken },
                    tag = objectToken.POSTag
                };
                triplets.objectTriplet = new ObjectTriplet() { chunk = objectChunk, confidence = 1 };
            }
            return triplets;
        }
    }
}
