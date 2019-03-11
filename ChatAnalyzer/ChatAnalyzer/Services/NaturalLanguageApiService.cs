using ChatModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatAnalyzer.Services
{
    public class NaturalLanguageApiService
    {
        private readonly HttpClient _httpClient;

        public NaturalLanguageApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Sentence>> AnalyzeMessageAsync(string message)
        {
            List<Sentence> sentences = null;

            var jsonString = JsonConvert.SerializeObject(message);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"api/AnalyzeMessage/", content);
            if (response.IsSuccessStatusCode)
            {
                sentences = await response.Content.ReadAsAsync<List<Sentence>>();
            }

            return sentences;
        }
    }
}
