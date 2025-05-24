namespace BlazorRAG.Models
{
    public class FaissSearchResult
    {
        public float[] vector { get; set; }
        public Dictionary<string, object> metadata { get; set; }
        public float distance { get; set; }
    }
}
