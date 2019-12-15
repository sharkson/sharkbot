using System.Net.Http;

namespace SharkbotApi.Services
{
    public class StalkerService
    {
        private readonly HttpClient _httpClient;

        public StalkerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void StalkUser(string user)
        {
            _httpClient.GetAsync($"api/RedditStalker/stalk?username={user}");
        }
    }
}
