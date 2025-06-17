using Refit;

namespace RefitGit.Models;

/// <summary>
/// Informações gerais sobre a taxa limite da API GitHub.
/// </summary>
public record RateLimitInfo(
    [property: AliasAs("rate")] RateDetails Rate
);

/// <summary>
/// Detalhes específicos da taxa limite.
/// </summary>
public record RateDetails(
    [property: AliasAs("limit")] int Limit,
    [property: AliasAs("remaining")] int Remaining,
    [property: AliasAs("reset")] long Reset
);
