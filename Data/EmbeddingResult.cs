using System.Collections.Generic;

namespace BlazorRAG.Data
{
    public class EmbeddingResult
    {
        public string Text { get; set; } = string.Empty;
        public float[] Embedding { get; set; } = new float[0];
    }
}
