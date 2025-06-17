using Refit;

namespace RefitGit.Models;

/// <summary>
/// Resultado da busca com total e lista de repositórios.
/// </summary>
public record SearchResult(
    [property: AliasAs("total_count")] int TotalCount,
    [property: AliasAs("items")] List<GitHubRepo> Items
);
