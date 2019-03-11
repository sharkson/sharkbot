using ChatModels;
using System;
using System.Collections.Generic;

namespace ConversationSteerService.Services
{
    public class EdgeService
    {
        public List<Tuple<string, string>> getEdges(List<ConversationList> conversationDatabase)
        {
            var edges = new List<Tuple<string, string>>();

            foreach (var conversationList in conversationDatabase)
            {
                foreach (var conversation in conversationList.conversations)
                {
                    foreach (var response in conversation.Value.responses)
                    {
                        foreach (var conversationSubject in response.naturalLanguageData.subjects)
                        {
                            if (response.naturalLanguageData.responseSubjects != null)
                            {
                                foreach (var responseConversationSubject in response.naturalLanguageData.responseSubjects)
                                {
                                    edges.Add(Tuple.Create(conversationSubject.Lemmas, responseConversationSubject.Lemmas));
                                }
                            }
                        }
                    }
                }
            }

            return edges;
        }
    }
}
