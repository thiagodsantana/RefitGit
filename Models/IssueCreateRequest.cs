using System.Text.Json.Serialization;

namespace RefitGit.Models
{
    public class IssueCreateRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("assignees")]
        public List<string>? Assignees { get; set; }

        [JsonPropertyName("labels")]
        public List<string>? Labels { get; set; }
    }
}
