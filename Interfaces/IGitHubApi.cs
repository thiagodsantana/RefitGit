using Microsoft.AspNetCore.Authentication.OAuth;
using Refit;
using RefitGit.Models;
using SearchResult = RefitGit.Models.SearchResult;

namespace RefitGit.Interfaces
{
    public interface IGitHubApi
    {
        // ---------------------------
        // Métodos existentes
        // ---------------------------

        [Get("/users/{username}")]
        Task<GitHubUser> GetUserAsync(string username);

        [Get("/users/{username}/repos")]
        Task<List<GitHubRepo>> GetUserReposAsync(string username);

        [Get("/repos/{owner}/{repo}")]
        Task<GitHubRepo> GetRepoAsync(string owner, string repo);

        [Get("/users/{username}/followers")]
        Task<List<GitHubUser>> GetFollowersAsync(string username);

        [Get("/users/{username}/following")]
        Task<List<GitHubUser>> GetFollowingAsync(string username);


        // ---------------------------
        // Exemplos adicionais Refit
        // ---------------------------

        /// <summary>
        /// Exemplo com múltiplos parâmetros de rota.
        /// </summary>
        [Get("/repos/{owner}/{repo}/issues/{issue_number}")]
        Task<Issue> GetIssueAsync(string owner, string repo, int issue_number);

        /// <summary>
        /// Exemplo com query string simples.
        /// </summary>
        [Get("/search/repositories")]
        Task<SearchResult> SearchRepositoriesAsync([Query] string q, [Query] int page = 1, [Query] int per_page = 10);

        /// <summary>
        /// Exemplo com objeto de query.
        /// </summary>
        [Get("/search/repositories")]
        Task<SearchResult> SearchRepositoriesWithObjectAsync([Query] SearchQuery query);

        /// <summary>
        /// Exemplo de rota fixa (sem parâmetros).
        /// </summary>
        [Get("/rate_limit")]
        Task<RateLimitInfo> GetRateLimitAsync();

        /// <summary>
        /// Exemplo com parâmetro opcional.
        /// </summary>
        [Get("/users/{username}/repos")]
        Task<List<GitHubRepo>> GetUserReposWithTypeAsync(string username, [Query] string? type = null);

        /// <summary>
        /// Exemplo de POST com body.
        /// </summary>
        [Post("/repos/{owner}/{repo}/issues")]
        Task<Issue> CreateIssueAsync(string owner, string repo, [Body] IssueCreateRequest request);

        /// <summary>
        /// Exemplo de PUT.
        /// </summary>
        [Put("/user/starred/{owner}/{repo}")]
        Task StarRepositoryAsync(string owner, string repo);

        /// <summary>
        /// Exemplo de DELETE.
        /// </summary>
        [Delete("/user/starred/{owner}/{repo}")]
        Task UnstarRepositoryAsync(string owner, string repo);

        /// <summary>
        /// Exemplo com Headers fixos por método.
        /// </summary>
        [Get("/user")]
        [Headers("Authorization: Bearer")]
        Task<GitHubUser> GetAuthenticatedUserAsync();

        /// <summary>
        /// Exemplo de upload de arquivo (multipart/form-data).
        /// </summary>
        [Multipart]
        [Post("/upload")]
        Task UploadFileAsync([AliasAs("file")] StreamPart file);

        /// <summary>
        /// Exemplo de Header dinâmico (enviado por parâmetro).
        /// </summary>
        [Get("/user")]
        Task<GitHubUser> GetUserWithDynamicHeaderAsync([Header("Authorization")] string authorization);

        /// <summary>
        /// Exemplo de PATCH (atualização parcial).
        /// </summary>
        [Patch("/repos/{owner}/{repo}")]
        Task<GitHubRepo> UpdateRepoPartialAsync(string owner, string repo, [Body] RepoUpdateRequest request);

        /// <summary>
        /// Exemplo de form-urlencoded (application/x-www-form-urlencoded).
        /// </summary>
        [Post("/login/oauth/access_token")]
        [Headers("Content-Type: application/x-www-form-urlencoded")]
        Task<OAuthTokenResponse> GetAccessTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        /// <summary>
        /// Exemplo de Full URL custom (usando URI completo, ignorando BaseUrl).
        /// </summary>
        [Get("")]
        Task<string> GetExternalContentAsync([AliasAs("Url")][Header("X-Base-Address")] string fullUrl);

        /// <summary>
        /// Exemplo usando CancellationToken.
        /// </summary>
        [Get("/users/{username}")]
        Task<GitHubUser> GetUserWithCancellationAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Exemplo de query string com array.
        /// </summary>
        [Get("/repos")]
        Task<List<GitHubRepo>> GetReposByIdsAsync([Query(CollectionFormat.Multi)] int[] repoIds);

        /// <summary>
        /// Exemplo de Accept header específico por método.
        /// </summary>
        [Get("/user/emails")]
        [Headers("Accept: application/vnd.github.v3+json")]
        Task<List<GitHubRepo>> GetUserEmailsAsync();        
    }
}
