using System;
using System.Collections.Generic;

namespace ChatModels
{
    [Serializable]
    public class NaturalLanguageData
    {
        public List<Sentence> sentences { get; set; }
        public List<ConversationSubject> subjects { get; set; }
        /// <summary>
        /// confidence level that this chat has a response
        /// </summary>
        public double responseConfidence { get; set; }     
        public List<ConversationSubject> responseSubjects { get; set; }
        public List<ConversationSubject> proximitySubjects { get; set; }
        public string userlessMessage { get; set; }
        public string AnalyzationVersion { get; set; }
    }
}