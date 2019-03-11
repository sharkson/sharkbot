using ChatModels;
using SimpleNetNlp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StanfordNaturalLanguage
{
    public class StanfordNaturalLanguageService
    {
        public StanfordNaturalLanguageService()
        {
            var document = new Document("I am the best ever.  How are you doing Ben?");
            foreach (var sentence in document.Sentences)
            {
                var lemmas = sentence.Lemmas;
                var pos = sentence.PosTags;
                var sentiment = sentence.Sentiment;
            }

            Debug.WriteLine("hi");
        }

        public NaturalLanguageData AnalyzeMessage(Chat chat)
        {
            var document = new Document(chat.message);
            var sentences = new List<ChatModels.Sentence>();

            foreach (var s in document.Sentences)
            {
                var sentence = new ChatModels.Sentence();

                sentence.tokens = new List<Token>();

                for (var index = 0; index < s.Words.Count; index++)
                {
                    var token = new Token();
                    token.POSTag = s.PosTags.ToList()[index];
                    token.Lexeme = s.Lemmas.ToList()[index];
                    token.Stem = s.Lemmas.ToList()[index];
                    sentence.tokens.Add(token);
                }

                sentence.interrogative = isInterrogative(s);

                sentence.chunks = new List<Chunk>();
                sentence.triplets = new Triplets();

                sentences.Add(sentence);
            }

            var naturalLanguageData = new NaturalLanguageData();
            naturalLanguageData.sentences = sentences;

            return naturalLanguageData;
        }

        private bool isInterrogative(SimpleNetNlp.Sentence sentence)
        {
            //TODO: do this better, for example they could say "that is what I mean" and the what would trigger it as a question but it shouldn't.
            if (sentence.PosTags.Any(t => t == "WP" || t == "WP$" || t == "WRB" || t == "?"))
            {
                return true;
            }
            return false;
        }
    }
}
