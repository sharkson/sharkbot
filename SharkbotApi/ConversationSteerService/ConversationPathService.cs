
using ChatModels;
using ConversationSteerService.Models;
using ConversationSteerService.Services;
using System.Collections.Generic;
using System.Linq;

namespace ConversationSteerService
{
    public class ConversationPathService
    {
        EdgeService edgeService;
        VerticeService verticeService;
        ShortestPathService shortestPathService;

        public ConversationPathService()
        {
            edgeService = new EdgeService();
            verticeService = new VerticeService();
            shortestPathService = new ShortestPathService();
        }

        public List<string> GetPathsSubjects(List<string> goals, List<string> starts, List<ConversationList> conversationLists)
        {
            var pathSubjects = new List<string>();
            pathSubjects.AddRange(goals);

            foreach (var start in starts) //TODO: parallelize for performance maybe
            {
                var vertices = verticeService.getVertices(conversationLists);
                var edges = edgeService.getEdges(conversationLists);

                var graph = new Graph<string>(vertices, edges);
                var shortestPathFunction = shortestPathService.GetShortestPathFunction(graph, start);
                foreach(var goal in goals)
                {
                    var shortestPath = shortestPathFunction(goal);
                    pathSubjects.AddRange(shortestPath);
                }
            }

            pathSubjects.RemoveAll(s => starts.Contains(s));

            return pathSubjects.Distinct().ToList();
        }
    }
}
