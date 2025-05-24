using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorRAG.Services
{
    public class CsvProcessingService
    {
        public async Task<List<string>> ExtractTextChunksAsync(Stream csvStream)
        {
            var result = new List<string>();
            using var reader = new StreamReader(csvStream);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.Add(line);
                }
            }
            return result;
        }
    }
}
