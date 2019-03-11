using ChatModels;
using System.IO;
using System.Collections.Generic;
using SimpleNetNlp;
using SimpleNetNlp.Exceptions;

namespace StanfordNaturalLanguageService
{
    public interface IMessageAnalyzationService
    {
        List<ChatModels.Sentence> AnalyzeMessage(string message);
    }

    public class MessageAnalyzationService : IMessageAnalyzationService
    {
        private readonly SentenceTypeService _sentenceTypeService;
        private readonly TokenService _tokenService;
        private readonly OpenieService _openieService;
        private readonly VoiceService _voiceService;
        private readonly SubjectService _subjectService;
        private readonly ObjectService _objectService;
        private readonly PredicateService _predicateService;
        private readonly SentimentAnalyzationService _sentimentAnalyzationService;

        public MessageAnalyzationService(string naturalLanguageDataRootDirectory, SentenceTypeService sentenceTypeService, TokenService tokenService, OpenieService openieService, VoiceService voiceService, SubjectService subjectService, ObjectService objectService, PredicateService predicateService, SentimentAnalyzationService sentimentAnalyzationService)
        {
            _sentenceTypeService = sentenceTypeService;
            _tokenService = tokenService;
            _openieService = openieService;
            _voiceService = voiceService;
            _subjectService = subjectService;
            _objectService = objectService;
            _predicateService = predicateService;
            _sentimentAnalyzationService = sentimentAnalyzationService;

            Directory.SetCurrentDirectory(naturalLanguageDataRootDirectory);
        }

        public List<ChatModels.Sentence> AnalyzeMessage(string message)
        {
            var sentences = new List<ChatModels.Sentence>();

            if(message == null)
            {
                message = string.Empty;
            }

            var document = new Document(message);
            foreach (var stanfordSentence in document.Sentences)
            {
                var source = stanfordSentence.ToString();
                var tokens = _tokenService.GetTokens(stanfordSentence);
                var openieTriples = new List<OpenieTriple>();
                try
                {
                    openieTriples = _openieService.GetOpenieTriples(stanfordSentence.OpenIe);
                }
                catch(UnhandledLibraryException simpleNlpException)
                {
                    openieTriples = new List<OpenieTriple>();
                }
                var sentenceType = _sentenceTypeService.GetSentenceType(tokens);
                var subject = _subjectService.GetSubject(openieTriples, tokens, sentenceType);
                var sentenceObject = _objectService.GetObject(openieTriples, tokens, sentenceType);
                var predicate = _predicateService.GetPredicate(openieTriples, tokens, sentenceType);

                var analyzedSentence = new ChatModels.Sentence
                {
                    Source = source,
                    Sentiment = _sentimentAnalyzationService.GetSentiment(source),
                    Tokens = tokens,
                    OpenieTriples = openieTriples,
                    SentenceType = sentenceType,
                    Voice = _voiceService.GetVoice(tokens),
                    Subject = subject,
                    Object = sentenceObject,
                    Predicate = predicate,
                };

                sentences.Add(analyzedSentence);
            }

            return sentences;
        }
    }
}
