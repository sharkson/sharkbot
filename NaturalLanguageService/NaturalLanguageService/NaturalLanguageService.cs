using ChatModels;
using Newtonsoft.Json.Linq;
using SharpNL;
using SharpNL.Analyzer;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NaturalLanguageService
{
    public static class NaturalLanguageService
    {
        private static AggregateAnalyzer analyzer;

        public static dynamic POSTagValues;

        public static List<string> LowValueNouns;

        public static void LoadAnalyzationData(string sent, string token, string pos, string chunker, string tags, string nouns)
        {
            analyzer = new AggregateAnalyzer { sent, token, pos, chunker };

            string[] lines = File.ReadAllLines(tags);
            POSTagValues = JObject.Parse(lines[0]);

            LowValueNouns = new List<string>(File.ReadAllLines(nouns));
        }

        public static NaturalLanguageData AnalyzeMessage(Chat chat)
        {
            var document = new Document("en", chat.message);
            var sentences = new List<Sentence>();

            try
            {
                analyzer.Analyze(document);
                foreach (var s in document.Sentences)
                {
                    var sentence = new Sentence();
                    sentence.tokens = new List<Token>();
                    foreach (var t in s.Tokens)
                    {
                        var token = new Token();
                        token.POSTag = t.POSTag;
                        token.Lexeme = t.Lexeme;
                        sentence.tokens.Add(token);
                    }

                    sentence.chunks = new List<Chunk>();
                    foreach (var c in s.Chunks)
                    {
                        var chunk = new Chunk();
                        chunk.tag = c.Tag;
                        chunk.tokens = new List<Token>();
                        foreach (var t in c.Tokens)
                        {
                            var token = new Token();
                            token.POSTag = t.POSTag;
                            token.Lexeme = t.Lexeme;
                            chunk.tokens.Add(token);
                        }
                        sentence.chunks.Add(chunk);
                    }

                    sentence.triplets = getSentenceTriplets(sentence.chunks);

                    sentence.interrogative = isInterrogative(s);

                    sentences.Add(sentence);
                }
            }
            catch (AnalyzerException)
            {

            }

            var naturalLanguageData = new NaturalLanguageData();
            naturalLanguageData.sentences = sentences;

            return naturalLanguageData;
        }

        public static List<string> GetSingularForms(string pluralWord)
        {
            var singularWords = new List<string>();
            if (pluralWord.EndsWith("ii"))
            {
                var index = pluralWord.LastIndexOf("ii");
                singularWords.Add(pluralWord.Remove(index, 2).Insert(index, "us"));
            }
            if (pluralWord.EndsWith("ies"))
            {
                var index = pluralWord.LastIndexOf("ies");
                singularWords.Add(pluralWord.Remove(index, 3).Insert(index, "y"));
            }
            if (pluralWord.EndsWith("es"))
            {
                var index = pluralWord.LastIndexOf("es");
                singularWords.Add(pluralWord.Remove(index, 2));
            }
            if (pluralWord.EndsWith("s"))
            {
                var index = pluralWord.LastIndexOf("s");
                singularWords.Add(pluralWord.Remove(index, 1));
            }

            return singularWords;
        }

        private static Triplets getSentenceTriplets(List<Chunk> chunks)
        {
            var triplets = new Triplets();

            //TODO: other patterns
            //noun-verb-nown = subject–verb–object
            if (chunks.FindIndex(c => c.tag == "NP") < chunks.FindIndex(c => c.tag == "VP") && chunks.FindIndex(c => c.tag == "VP") < chunks.FindLastIndex(c => c.tag == "NP"))
            {
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
            }
            else if (chunks.FindIndex(c => c.tag == "NP") < chunks.FindIndex(c => c.tag == "VP"))
            {
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
            }

            return triplets;
        }

        private static bool isInterrogative(SharpNL.SentenceDetector.Sentence sentence)
        {
            //TODO: do this better, for example they could say "that is what I mean" and the what would trigger it as a question but it shouldn't.
            if (sentence.Tokens.Any(t => t.POSTag == "WP" || t.POSTag == "WP$" || t.POSTag == "WRB" || t.Lexeme == "?"))
            {
                return true;
            }
            return false;
        }
    }
}
