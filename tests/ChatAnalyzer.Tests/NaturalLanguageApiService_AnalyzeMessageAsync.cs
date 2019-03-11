using ChatAnalyzer.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ChatAnalyzer.Tests
{
    public class NaturalLanguageApiService_AnalyzeMessageAsync
    {
        [Fact]
        public async Task AnalyzeMessageAsync()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:50214/")
            };
            var service = new NaturalLanguageApiService(client);
            var result = await service.AnalyzeMessageAsync("hello world");
            Assert.Single(result);
        }
    }
}
