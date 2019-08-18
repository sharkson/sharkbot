using ChatModels;
using System.Linq;
using VaderSharp;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class MessageAnalyzationService_AnalyzeMessage
    {
        [Fact]
        public void CorrectSentenceData()
        {
            var service = GetService();

            var message = "Sharks are in the Mississippi River.  They swim fast!  How are you doing?  Swim faster.";

            var sentences = service.AnalyzeMessage(message);
            Assert.Equal(4, sentences.Count);

            var sentence = sentences.FirstOrDefault();
            Assert.Equal(7, sentence.Tokens.Count);
            Assert.Equal(SentenceType.Declarative, sentence.SentenceType);

            var token = sentence.Tokens.FirstOrDefault();
            Assert.Equal("Sharks", token.Word);
            Assert.Equal("shark", token.Lemmas);
            Assert.Equal("NNS", token.PosTag);
            Assert.Equal("O", token.NerTag);

            var sentence2 = sentences[1];
            Assert.Equal(SentenceType.Exclamatory, sentence2.SentenceType);

            var sentence3 = sentences[2];
            Assert.Equal(SentenceType.Interrogative, sentence3.SentenceType);

            var sentence4 = sentences[3];
            Assert.Equal(SentenceType.Imperative, sentence4.SentenceType);
        }

        [Fact]
        public void DeclarativeSentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "I live at 24, East street.  I like ice-cream.  The wind is blowing from the west. He runs. She sings. I like climbing. Fran is sad. My cat is black. Dogs are cute. He is eight years old. The sky is blue. He loves pizza. The car is white. Ice is cold. He wanted to play football, but she wanted to play basketball. Marie loves the beach, yet she hates sand. She plays the piano, and he sings along. She had to make the next flight; she quickly packed her bag. The house has new windows; however, the roof still leaks. It had rained for days; the town was flooded. She leaves for college tomorrow morning. The weather is warm and sunny; a perfect day for a picnic. She wears red nail polish. The room smells clean. I love my cat. My family is driving to the beach for the long weekend. The airplane flew over the gleaming ocean. She is my friend. His shoes were brand new, and now they are missing. The dog chased the boy. It is a nice day. Her sister is sick; therefore, she is not at school today. The grass is green after the rain. She loves the mountains; he hates the long drive. My new dress is black and white. My brother loves to run, but my sister prefers to walk. My phone is missing. The teacher is going on a well-earned vacation. Her coat is ripped. The baby is hungry; she is eagerly drinking a bottle of milk.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.SentenceType == SentenceType.Declarative));
        }

        [Fact]
        public void ExclamatorySentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "Wow, she must love scuba diving!  Red sea blue holes are out of this world!  I can’t believe she ran that fast to grab the bone! Happy birthday, Amy! Thank you, Sheldon! I hate you! Ice cream sundaes are my favorite! Wow, I really love you! Fantastic, let's go! What a lovely bouquet of flowers! What a cute puppy! What an ugly bug! What a happy ending! How bright they've grown in the sunlight! How well he listens! How slow they crawl! How fast you ran! That birthday cake was so good! Sheldon's gift was so amazing! Eugh, that bug is so ugly! I'm so mad right now! He's such a kind soul! That's such a gorgeous ring! Your puppy is such a cutie! You're such a liar!",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.SentenceType == SentenceType.Exclamatory));
        }

        [Fact]
        public void ImperativeSentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "Come to the ball dance with me!  Sop moving in circles!  Move in circle just once a day.  Have fun at the ball dance!  Please get out of the room! Pass the salt. Move out of my way! Shut the front door. Find my leather jacket. Be there at five. Clean your room. Complete these by tomorrow. Consider the red dress. Wait for me. Get out! Make sure you pack warm clothes. Choose Eamonn, not Seamus. Please be quiet. Be nice to your friends.Play ball! Preheat the oven. Don't eat all the cookies. Stop feeding the dog from the table. Come out with us tonight. Please join us for dinner. Choose the Irish wolfhound, not the German Shepherd. Wear your gold necklace with that dress. Get out of here!", 
                user = "tester"
            };
            //"Use oil in the pan." stanford nlp thinks Use is a proper noun instead of a verb
            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.SentenceType == SentenceType.Imperative));
        }

        [Fact]
        public void InterrogativeSentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "When do you get off work?  Who do you trust the most in the world?  Where do you live in California?  Which city is your favourite?  How can I get to this karate teacher?  Is she a student of Arts?  Do you like to eat ice-cream?  What is love?",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.SentenceType == SentenceType.Interrogative));
        }

        [Fact]
        public void ActiveSentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "Harry ate six shrimp at dinner. Beautiful giraffes roam the savannah. Sue changed the flat tire. We are going to watch a movie tonight. I ran the obstacle course in record time. The crew paved the entire stretch of highway. Mom read the novel in one day. I will clean the house every Saturday. The staff is required to watch a safety video every year. Tom painted the entire house. The teacher always answers the students' questions. The choir really enjoys that piece. The forest fire destroyed the whole suburb. The two kings are signing the treaty. Larry generously donated money to the homeless shelter. The wedding planner is making all the reservations. Susan will bake two dozen cupcakes for the bake sale.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Voice == Voice.Active));
        }

        [Fact]
        public void PassiveSentences()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "At dinner, six shrimp were eaten by Harry. The savannah is roamed by beautiful giraffes. The flat tire was changed by Sue. A movie is going to be watched by us tonight. The obstacle course was run by me in record time. The entire stretch of highway was paved by the crew. The novel was read by Mom in one day. The house will be cleaned by me every Saturday. A safety video will be watched by the staff every year. The entire house was painted by Tom. The students' questions are always answered by the teacher.  That piece is really enjoyed by the choir. The whole suburb was destroyed by the forest fire. The treaty is being signed by the two kings. Every night, the office is vacuumed and dusted by the cleaning crew. Money was generously donated to the homeless shelter by Larry. All the reservations are being made by the wedding planner.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Voice == Voice.Passive));
        }

        [Fact]
        public void HandlesOpenieTripleException()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "i better wait a bit before another",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.OpenieTriples != null));
        }

        [Fact]
        public void HandlesWeirdCharacters()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "尸尺乇尸卂尺乇 下口尺 丅尺口凵乃乚乇 卂𠘨刀 从卂长乇 工丅 刀口凵乃乚乇!",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences != null);
        }

        [Fact]
        public void HandlesEmojis()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "I'm a 37.5% 🤔 ",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences != null);
        }

        [Fact]
        public void HandlesNull()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = null,
                message = null,
                user = null
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences != null);
        }

        [Fact]
        public void Subjects()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "I always win.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Subject.Word == "I"));

            chat.message = "The shark swims fast";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Subject.Word == "shark"));

            chat.message = "Clean the dishes";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Subject.Word == "you"));
        }

        [Fact]
        public void Objects()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "I always win money.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "money"));

            chat.message = "The shark swims fast";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == null));

            chat.message = "Clean the dishes.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "dishes"));

            chat.message = "Paint the kitchen.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "kitchen"));

            chat.message = "Poop in the toilet.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "toilet"));

            chat.message = "Wash your hands.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "hands"));

            chat.message = "Harry ate six shrimp at dinner.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Object.Word == "shrimp"));   
        }

        [Fact]
        public void Predicates()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "I always win money.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Predicate.Word == "win"));

            chat.message = "The shark swims fast";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Predicate.Word == "swims"));

            chat.message = "Clean the dishes.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Predicate.Word == "Clean"));

            chat.message = "Paint the kitchen.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Predicate.Word == "Paint"));
        }

        [Fact]
        public void Sentiment()
        {
            var service = GetService();
            var chat = new Chat
            {
                botName = "sharkbot",
                message = "This sentiment anaylzer works great! I love it.",
                user = "tester"
            };

            var sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Sentiment > 0));

            chat.message = "The Stanford sentiment anaylzer is terrible. I hate it.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Sentiment < 0));

            chat.message = "I do sentiment analyzation. Brush your teeth.";
            sentences = service.AnalyzeMessage(chat.message);
            Assert.True(sentences.All(s => s.Sentiment == 0));
        }

        private MessageAnalyzationService GetService()
        {
            var modelRootFolder = @"C:\sharkbot\stanford-corenlp-3.9.1-models";
            return new MessageAnalyzationService(modelRootFolder, new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService()), new TokenService(), new OpenieService(), new VoiceService(), new SubjectService(), new ObjectService(), new PredicateService(), new SentimentAnalyzationService(new SentimentIntensityAnalyzer()));
        }
    }
}
