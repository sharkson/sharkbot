
using ChatModels;
using System.Collections.Generic;

namespace ConversationSteerService.Services
{
    public class ConversationPathService
    {
        public List<string> GetPath(string goal, string start, List<ConversationList> conversationDatabase)
        {
            //TODO: find the shortest path from the start to the goal, also pre-compute subject and response subject as part of natural language data

            return new List<string>();
        }
    }
}
