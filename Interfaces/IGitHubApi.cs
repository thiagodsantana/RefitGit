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
        Task<ApiResponse<GitHubUser>> GetUserAsync(string username);

        [Get("/users/{username}/repos")]
        Task<ApiResponse<List<GitHubRepo>>> GetUserReposAsync(string username);

        [Get("/repos/{owner}/{repo}")]
        Task<ApiResponse<GitHubRepo>> GetRepoAsync(string owner, string repo);

        [Get("/users/{username}/followers")]
        Task<ApiResponse<List<GitHubUser>>> GetFollowersAsync(string username);

        [Get("/users/{username}/following")]
        Task<ApiResponse<List<GitHubUser>>> GetFollowingAsync(string username);


        // ---------------------------
        // Exemplos adicionais Refit
        // ---------------------------

        /// <summary>
        /// Exemplo com múltiplos parâmetros de rota.
        /// </summary>
        [Get("/repos/{owner}/{repo}/issues/{issue_number}")]
        Task<ApiResponse<Issue>> GetIssueAsync(string owner, string repo, int issue_number);

        /// <summary>
        /// Exemplo com query string simples.
        /// </summary>
        [Get("/search/repositories")]
        Task<ApiResponse<SearchResult>> SearchRepositoriesAsync([Query] string q, [Query] int page = 1, [Query] int per_page = 10);

        /// <summary>
        /// Exemplo com objeto de query.
        /// </summary>
        [Get("/search/repositories")]
        Task<ApiResponse<SearchResult>> SearchRepositoriesWithObjectAsync([Query] SearchQuery query);

        /// <summary>
        /// Exemplo de rota fixa (sem parâmetros).
        /// </summary>
        [Get("/rate_limit")]
        Task<ApiResponse<RateLimitInfo>> GetRateLimitAsync();

        /// <summary>
        /// Exemplo com parâmetro opcional.
        /// </summary>
        [Get("/users/{username}/repos")]
        Task<ApiResponse<List<GitHubRepo>>> GetUserReposWithTypeAsync(string username, [Query] string? type = null);

        /// <summary>
        /// Exemplo de POST com body.
        /// </summary>
        [Post("/repos/{owner}/{repo}/issues")]
        Task<ApiResponse<Issue>> CreateIssueAsync(string owner, string repo, [Body] IssueCreateRequest request);

        /// <summary>
        /// Exemplo de PUT.
        /// </summary>
        [Put("/user/starred/{owner}/{repo}")]
        Task<ApiResponse<object>> StarRepositoryAsync(string owner, string repo);

        /// <summary>
        /// Exemplo de DELETE.
        /// </summary>
        [Delete("/user/starred/{owner}/{repo}")]
        Task<ApiResponse<object>> UnstarRepositoryAsync(string owner, string repo);

        /// <summary>
        /// Exemplo com Headers fixos por método.
        /// </summary>
        [Get("/user")]
        Task<ApiResponse<GitHubUser>> GetAuthenticatedUserAsync([Authorize] string authorization);
        Task<ApiResponse<GitHubUser>> GetAuthenticatedUserAsync();

        /// <summary>
        /// Exemplo de upload de arquivo (multipart/form-data).
        /// </summary>
        [Multipart]
        [Post("/upload")]
        Task<ApiResponse<object>> UploadFileAsync([AliasAs("file")] StreamPart file);

        /// <summary>
        /// Exemplo de Header dinâmico (enviado por parâmetro).
        /// </summary>
        [Get("/user")]
        Task<ApiResponse<GitHubUser>> GetUserWithDynamicHeaderAsync([Header("Authorization")] string authorization);

        /// <summary>
        /// Exemplo de PATCH (atualização parcial).
        /// </summary>
        [Patch("/repos/{owner}/{repo}")]
        Task<ApiResponse<GitHubRepo>> UpdateRepoPartialAsync(string owner, string repo, [Body] RepoUpdateRequest request);

        /// <summary>
        /// Exemplo de form-urlencoded (application/x-www-form-urlencoded).
        /// </summary>
        [Post("/login/oauth/access_token")]
        [Headers("Content-Type: application/x-www-form-urlencoded")]
        Task<ApiResponse<OAuthTokenResponse>> GetAccessTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        /// <summary>
        /// Exemplo de Full URL custom (usando URI completo, ignorando BaseUrl).
        /// </summary>
        [Get("")]
        Task<ApiResponse<string>> GetExternalContentAsync([AliasAs("Url")][Header("X-Base-Address")] string fullUrl);

        /// <summary>
        /// Exemplo usando CancellationToken.
        /// </summary>
        [Get("/users/{username}")]
        Task<ApiResponse<GitHubUser>> GetUserWithCancellationAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Exemplo de query string com array.
        /// </summary>
        [Get("/repos")]
        Task<ApiResponse<List<GitHubRepo>>> GetReposByIdsAsync([Query(CollectionFormat.Multi)] int[] repoIds);

        /// <summary>
        /// Exemplo de Accept header específico por método.
        /// </summary>
        [Get("/user/emails")]
        [Headers("Accept: application/vnd.github.v3+json")]
        Task<ApiResponse<List<GitHubRepo>>> GetUserEmailsAsync();
    }
}
