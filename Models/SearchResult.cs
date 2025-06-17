using System.Text.Json.Serialization;

namespace RefitGit.Models
{
    public class SearchResult
    {
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("items")]
        public List<GitHubRepo> Items { get; set; }
    }
}
