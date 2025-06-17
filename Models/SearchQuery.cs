namespace RefitGit.Models
{
    public class SearchQuery
    {
        public string Q { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int Per_Page { get; set; } = 10;
    }
}
