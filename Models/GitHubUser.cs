using Refit;

namespace RefitGit.Models;

/// <summary>
/// Representa um usuário do GitHub.
/// </summary>
public record class GitHubUser(
    [property: AliasAs("login")] string Login,
    [property: AliasAs("id")] long Id,
    [property: AliasAs("avatar_url")] string AvatarUrl,
    [property: AliasAs("html_url")] string HtmlUrl
);
