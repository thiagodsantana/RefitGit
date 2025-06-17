using Refit;

namespace RefitGit.Models;

/// <summary>
/// Representa uma issue do GitHub.
/// </summary>
public record class Issue(
    [property: AliasAs("id")] long Id,
    [property: AliasAs("number")] int Number,
    [property: AliasAs("title")] string Title,
    [property: AliasAs("body")] string? Body,
    [property: AliasAs("state")] string State,
    [property: AliasAs("user")] GitHubUser User
);
