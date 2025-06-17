using Refit;

namespace RefitGit.Models;

/// <summary>
/// Representa um repositório do GitHub.
/// </summary>
public record class GitHubRepo(
    [property: AliasAs("id")] long Id,
    [property: AliasAs("name")] string Name,
    [property: AliasAs("full_name")] string FullName,
    [property: AliasAs("html_url")] string HtmlUrl,
    [property: AliasAs("description")] string? Description,
    [property: AliasAs("owner")] GitHubUser Owner
);
