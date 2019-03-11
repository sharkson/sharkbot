//using SimpleNetNlp;

//namespace StanfordNaturalLanguage
//{
//    public class StanfordNaturalLanguageService
//    {
//        public StanfordNaturalLanguageService()
//        {
//            var document = new Document("I am the best ever.  How are you doing Ben?");
//            foreach (var sentence in document.Sentences)
//            {
//                var lemmas = sentence.Lemmas;
//                var pos = sentence.PosTags;
//                var sentiment = sentence.Sentiment;
//            }

//            Debug.WriteLine("hi");
//        }

//        public NaturalLanguageData AnalyzeMessage(Chat chat)
//        {
//            var document = new Document(chat.message);
//            var sentences = new List<ChatModels.Sentence>();

//            try
//            {
//                analyzer.Analyze(document);
//                foreach (var s in document.Sentences)
//                {
//                    var sentence = new Sentence();
//                    sentence.tokens = new List<Token>();
//                    foreach (var t in s.Tokens)
//                    {
//                        var token = new Token();
//                        token.POSTag = t.POSTag;
//                        token.Lexeme = t.Lexeme;
//                        token.Stem = wordStemmer.GetSteamWord(t.Lexeme);
//                        sentence.tokens.Add(token);
//                    }

//                    sentence.chunks = new List<Chunk>();
//                    foreach (var c in s.Chunks)
//                    {
//                        var chunk = new Chunk();
//                        chunk.tag = c.Tag;
//                        chunk.tokens = new List<Token>();
//                        foreach (var t in c.Tokens)
//                        {
//                            var token = new Token();
//                            token.POSTag = t.POSTag;
//                            token.Lexeme = t.Lexeme;
//                            token.Stem = wordStemmer.GetSteamWord(t.Lexeme);
//                            chunk.tokens.Add(token);
//                        }
//                        sentence.chunks.Add(chunk);
//                    }

//                    sentence.interrogative = isInterrogative(s);

//                    sentence.triplets = tripletService.GetSentenceTriplets(sentence);

//                    sentences.Add(sentence);
//                }
//            }
//            catch (AnalyzerException)
//            {

//            }

//            var naturalLanguageData = new NaturalLanguageData();
//            naturalLanguageData.sentences = sentences;

//            return naturalLanguageData;
//        }
//    }
//}
