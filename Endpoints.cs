using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Refit;
using RefitGit.Interfaces;
using RefitGit.Models;

namespace RefitGit
{
    [ApiExplorerSettings(GroupName = "v1")]
    public static class Endpoints
    {
        public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            // GET /users/{username}
            group.MapGet("/users/{username}", async (string username, IGitHubApi api, IMemoryCache cache) =>
            {
                try
                {
                    var cacheKey = $"user:{username}";
                    if (cache.TryGetValue(cacheKey, out GitHubUser? user))
                    {
                        return Results.Ok(user);
                    }

                    user = await api.GetUserAsync(username);
                    cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
                    return Results.Ok(user);
                }
                catch (ApiException ex)
                {
                    return Results.Problem($"Erro na API GitHub: {ex.StatusCode} - {ex}");
                }
            })
            .WithOpenApi();

            group.MapGet("/users/{username}/repos", async (string username, IGitHubApi api, IMemoryCache cache) =>
            {
                try
                {
                    var cacheKey = $"repos:{username}";
                    if (cache.TryGetValue(cacheKey, out List<GitHubRepo>? repos))
                        return Results.Ok(repos);

                    repos = await api.GetUserReposAsync(username);
                    cache.Set(cacheKey, repos, TimeSpan.FromMinutes(5));
                    return Results.Ok(repos);
                }
                catch (ApiException ex)
                {
                    return Results.Problem($"Erro na API GitHub: {ex.StatusCode} - {ex.Message}");
                }
            })
            .WithOpenApi();

            group.MapGet("/repos/{owner}/{repo}", async (string owner, string repo, IGitHubApi api, IMemoryCache cache) =>
            {
                try
                {
                    var cacheKey = $"repo:{owner}:{repo}";
                    if (cache.TryGetValue(cacheKey, out GitHubRepo? repoInfo))
                        return Results.Ok(repoInfo);

                    repoInfo = await api.GetRepoAsync(owner, repo);
                    cache.Set(cacheKey, repoInfo, TimeSpan.FromMinutes(5));
                    return Results.Ok(repoInfo);
                }
                catch (ApiException ex)
                {
                    return Results.Problem($"Erro na API GitHub: {ex.StatusCode} - {ex.Message}");
                }
            })
            .WithOpenApi();

            group.MapGet("/users/{username}/followers", async (string username, IGitHubApi api, IMemoryCache cache) =>
            {
                try
                {
                    var cacheKey = $"followers:{username}";
                    if (cache.TryGetValue(cacheKey, out List<GitHubUser>? followers))
                        return Results.Ok(followers);

                    followers = await api.GetFollowersAsync(username);
                    cache.Set(cacheKey, followers, TimeSpan.FromMinutes(5));
                    return Results.Ok(followers);
                }
                catch (ApiException ex)
                {
                    return Results.Problem($"Erro na API GitHub: {ex.StatusCode} - {ex.Message}");
                }
            })
            .WithOpenApi();

            group.MapGet("/users/{username}/following", async (string username, IGitHubApi api, IMemoryCache cache) =>
            {
                try
                {
                    var cacheKey = $"following:{username}";
                    if (cache.TryGetValue(cacheKey, out List<GitHubUser>? following))
                        return Results.Ok(following);

                    following = await api.GetFollowingAsync(username);
                    cache.Set(cacheKey, following, TimeSpan.FromMinutes(5));
                    return Results.Ok(following);
                }
                catch (ApiException ex)
                {
                    return Results.Problem($"Erro na API GitHub: {ex.StatusCode} - {ex.Message}");
                }
            })
            .WithOpenApi();

            return group;
        }
    }
}
