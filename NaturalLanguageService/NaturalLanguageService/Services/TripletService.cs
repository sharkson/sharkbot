using ChatModels;

namespace NaturalLanguageService.Services
{
    public class TripletService
    {
        private readonly ReplyTripletService _replyTripletService;
        private readonly QuestionTripletService _questionTripletService;

        public TripletService(ReplyTripletService replyTripletService, QuestionTripletService questionTripletService)
        {
            _replyTripletService = replyTripletService;
            _questionTripletService = questionTripletService;
        }

        public Triplets GetSentenceTriplets(Sentence sentence)
        {
            var triplets = new Triplets();

            if (!sentence.interrogative)
            {
                triplets = _replyTripletService.GetReplyTriplets(sentence);
            }
            else
            {
                triplets = _questionTripletService.GetQuestionTriplets(sentence);
            }

            return triplets;
        }
    }
}
