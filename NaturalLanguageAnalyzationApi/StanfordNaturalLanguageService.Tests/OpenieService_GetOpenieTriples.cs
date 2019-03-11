using SimpleNetNlp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace StanfordNaturalLanguageService.Tests
{
    public class OpenieService_GetOpenieTriples
    {
        [Fact]
        public void CorrectTriples()
        {
            var service = new OpenieService();
            var relationTriples = new List<RelationTriple>();
            var triples = service.GetOpenieTriples(new ReadOnlyCollection<RelationTriple>(relationTriples));
            Assert.Empty(triples);
        }
    }
}
