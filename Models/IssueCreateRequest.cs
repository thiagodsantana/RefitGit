using Refit;

namespace RefitGit.Models;

/// <summary>
/// Dados para criação de uma issue no GitHub.
/// </summary>
public record IssueCreateRequest(
    [property: AliasAs("title")] string Title,
    [property: AliasAs("body")] string? Body,
    [property: AliasAs("assignees")] List<string>? Assignees,
    [property: AliasAs("labels")] List<string>? Labels
);
