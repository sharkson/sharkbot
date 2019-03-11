using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace ConversationSteerService.Services
{
    public class VerticeService
    {
        public List<string> getVertices(List<ConversationList> conversationDatabase)
        {
            var vertices = new List<string>();

            foreach (var conversationList in conversationDatabase)
            {
                foreach (var conversation in conversationList.conversations)
                {
                    foreach (var conversationSubject in conversation.Value.subjects)
                    {
                        vertices.Add(conversationSubject.Lemmas);
                    }
                }
            }

            return vertices.Distinct().ToList();
        }
    }
}
