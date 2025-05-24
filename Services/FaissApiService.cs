using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Linq;
using BlazorRAG.Models;

namespace BlazorRAG.Services
{
    public class FaissApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _faissApiUrl;
        public FaissApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _faissApiUrl = config["FaissApi:Url"] ?? "http://localhost:8000";
        }

        public async Task UploadEmbeddingsAsync(string fileName, List<float[]> embeddings, List<string> texts)
        {
            var payload = new { fileName, embeddings, texts };
            await _httpClient.PostAsJsonAsync($"{_faissApiUrl}/add", payload);
        }

        public async Task<List<string>> QueryAsync(float[] queryEmbedding, int topK = 5)
        {
            var payload = new { query = queryEmbedding, top_k = topK };
            var response = await _httpClient.PostAsJsonAsync($"{_faissApiUrl}/search", payload);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<FaissSearchResponse>();
            // Extract the relevant string from metadata, e.g., "text"
            return result?.results?.Select(r => r.metadata.TryGetValue("text", out var text) ? text?.ToString() : null)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList() ?? new List<string>();
        }
    }
}
