using ChatModels;
using Newtonsoft.Json.Linq;
using SharpNL;
using SharpNL.Analyzer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Annytab.Stemmer;
using NaturalLanguageService.Services;

namespace NaturalLanguageService
{
    public static class NaturalLanguageService
    {
        private static AggregateAnalyzer analyzer;

        private static EnglishStemmer wordStemmer;

        private static TripletService tripletService;

        public static dynamic POSTagValues;

        public static List<string> LowValueNouns;

        public static void LoadAnalyzationData(string sent, string token, string pos, string chunker, string tags, string nouns)
        {
            analyzer = new AggregateAnalyzer { sent, token, pos, chunker };
            wordStemmer = new EnglishStemmer();
            tripletService = new TripletService(new ReplyTripletService(), new QuestionTripletService());

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
                        token.Stem = wordStemmer.GetSteamWord(t.Lexeme);
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
                            token.Stem = wordStemmer.GetSteamWord(t.Lexeme);
                            chunk.tokens.Add(token);
                        }
                        sentence.chunks.Add(chunk);
                    }

                    sentence.interrogative = isInterrogative(s);

                    sentence.triplets = tripletService.GetSentenceTriplets(sentence);                    

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
