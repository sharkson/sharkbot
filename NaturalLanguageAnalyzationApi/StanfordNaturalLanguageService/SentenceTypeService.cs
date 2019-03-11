using ChatModels;
using System.Collections.Generic;

namespace StanfordNaturalLanguageService
{
    public class SentenceTypeService
    {
        private readonly InterrogativeService _interrogativeService;
        private readonly DeclarativeService _declarativeService;
        private readonly ImperativeService _imperativeService;
        private readonly ExclamatoryService _exclamatoryService;

        public SentenceTypeService(InterrogativeService interrogativeService, DeclarativeService declarativeService, ImperativeService imperativeService, ExclamatoryService exclamatoryService)
        {
            _interrogativeService = interrogativeService;
            _declarativeService = declarativeService;
            _imperativeService = imperativeService;
            _exclamatoryService = exclamatoryService;
        }

        public SentenceType GetSentenceType(List<Token> tokens)
        {
            if (_interrogativeService.IsInterrogative(tokens))
            {
                return SentenceType.Interrogative;
            }

            if (_imperativeService.IsImperative(tokens))
            {
                return SentenceType.Imperative;
            }

            if (_exclamatoryService.IsExclamatory(tokens))
            {
                return SentenceType.Exclamatory;
            }

            if(_declarativeService.IsDeclarative(tokens))
            {
                return SentenceType.Declarative;
            }

            return SentenceType.Unidentifiable;
        }
    }
}
