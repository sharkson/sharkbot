using System.Collections.Generic;
using ChatModels;
using SimpleNetNlp;

namespace StanfordNaturalLanguageService
{
    public class OpenieService
    {
        public List<OpenieTriple> GetOpenieTriples(IReadOnlyCollection<RelationTriple> openIe)
        {
            var triples = new List<OpenieTriple>();
            foreach(var triple in openIe)
            {
                triples.Add(new OpenieTriple { Subject = triple.Subject, Object = triple.Object, Relation = triple.Relation });
            }
            return triples;
        }
    }
}
