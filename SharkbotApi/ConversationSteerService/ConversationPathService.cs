
using ChatModels;
using ConversationSteerService.Models;
using ConversationSteerService.Services;
using System.Collections.Generic;
using System.Linq;

namespace ConversationSteerService
{
    public class ConversationPathService
    {
        private readonly EdgeService _edgeService;
        private readonly VerticeService _verticeService;
        private readonly ShortestPathService _shortestPathService;

        public ConversationPathService(EdgeService edgeService, VerticeService verticeService, ShortestPathService shortestPathService)
        {
            _edgeService =edgeService;
            _verticeService = verticeService;
            _shortestPathService = shortestPathService;
        }

        public List<string> GetPathsSubjects(List<string> goals, List<string> starts, List<ConversationList> conversationLists)
        {
            var pathSubjects = new List<string>();
            pathSubjects.AddRange(goals);

            if(goals.Count == 0 || starts.Count == 0)
            {
                return pathSubjects;
            }

            foreach (var start in starts) //TODO: parallelize for performance maybe
            {
                var vertices = _verticeService.getVertices(conversationLists);
                var edges = _edgeService.getEdges(conversationLists);

                var graph = new Graph<string>(vertices, edges);
                var shortestPathFunction = _shortestPathService.GetShortestPathFunction(graph, start);
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
