using System.Text.Json.Serialization;

namespace RefitGit.Models
{
    public class Issue
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("user")]
        public GitHubUser User { get; set; }
    }
}
