using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlazorRAG.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public OpenAIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenAI:ApiKey"] ?? "";
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var payload = new
            {
                input = text,
                model = "text-embedding-3-small"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/embeddings")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var arr = doc.RootElement.GetProperty("data")[0].GetProperty("embedding");
            return arr.EnumerateArray().Select(x => x.GetSingle()).ToArray();
        }

        public async Task<string> GetAnswerAsync(string query, List<string> contextChunks)
        {
            var prompt = $"Answer the following question using the provided context.\nContext:\n{string.Join("\n", contextChunks)}\nQuestion: {query}";
            var payload = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 512
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
        }
    }
}
