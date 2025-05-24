using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace BlazorRAG.Services
{
    public class QueryCacheService
    {
        private readonly ConcurrentDictionary<string, string> _cache = new();
        public bool TryGet(string query, out string answer) => _cache.TryGetValue(query, out answer);
        public void Set(string query, string answer) => _cache[query] = answer;
    }
}
