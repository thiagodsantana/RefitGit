using System.Text.Json.Serialization;

namespace RefitGit.Models
{
    public class RateLimitInfo
    {
        [JsonPropertyName("rate")]
        public RateDetails Rate { get; set; }
    }

    public class RateDetails
    {
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }

        [JsonPropertyName("reset")]
        public long Reset { get; set; } // Unix timestamp
    }
}
