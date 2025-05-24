using System.Text;

namespace BlazorRAG.Services
{
    public static class TokenUtils
    {
        // Simple token count: count whitespace-separated words (for demo; replace with tiktoken for production)
        public static int CountTokens(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        // Trim context chunks to fit within a token limit
        public static List<string> TrimToTokenLimit(List<string> chunks, int maxTokens)
        {
            var result = new List<string>();
            int total = 0;
            foreach (var chunk in chunks)
            {
                int tokens = CountTokens(chunk);
                if (total + tokens > maxTokens) break;
                result.Add(chunk);
                total += tokens;
            }
            return result;
        }
    }
}
