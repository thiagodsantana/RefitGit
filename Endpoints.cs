using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RefitGit.Interfaces;
using RefitGit.Models;

namespace RefitGit;

[ApiExplorerSettings(GroupName = "v1")]
public static class Endpoints
{
    public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        // GET /users/{username}
        group.MapGet("/users/{username}", async (string username, IGitHubApi api, IMemoryCache cache) =>
        {
            var cacheKey = $"user:{username}";
            if (cache.TryGetValue(cacheKey, out GitHubUser? user))
            {
                return Results.Ok(user);
            }

            var response = await api.GetUserAsync(username);
            if (response.IsSuccessStatusCode)
            {
                cache.Set(cacheKey, response.Content, TimeSpan.FromMinutes(5));
                return Results.Ok(response.Content);
            }
            return Results.Problem($"Erro na API GitHub: {response.StatusCode} - {response.Error}");

        })
        .WithOpenApi();

        group.MapGet("/users/{username}/repos", async (string username, IGitHubApi api, IMemoryCache cache) =>
        {
            var cacheKey = $"repos:{username}";
            if (cache.TryGetValue(cacheKey, out List<GitHubRepo>? repos))
                return Results.Ok(repos);

            var response = await api.GetUserReposAsync(username);
            if (response.IsSuccessStatusCode)
            {
                cache.Set(cacheKey, repos, TimeSpan.FromMinutes(5));
                return Results.Ok(response.Content);
            }
            return Results.Problem($"Erro na API GitHub: {response.StatusCode} - {response.Error}");
        })
        .WithOpenApi();

        group.MapGet("/repos/{owner}/{repo}", async (string owner, string repo, IGitHubApi api, IMemoryCache cache) =>
            {

                var cacheKey = $"repo:{owner}:{repo}";
                if (cache.TryGetValue(cacheKey, out GitHubRepo? repoInfo))
                    return Results.Ok(repoInfo);

                var response = await api.GetRepoAsync(owner, repo);
                if (response.IsSuccessStatusCode)
                {
                    cache.Set(cacheKey, response.Content, TimeSpan.FromMinutes(5));
                    return Results.Ok(response.Content);
                }
                return Results.Problem($"Erro na API GitHub: {response.StatusCode} - {response.Error}");

            })
            .WithOpenApi();

        group.MapGet("/users/{username}/followers", async (string username, IGitHubApi api, IMemoryCache cache) =>
        {

            var cacheKey = $"followers:{username}";
            if (cache.TryGetValue(cacheKey, out List<GitHubUser>? followers))
                return Results.Ok(followers);

            var response = await api.GetFollowersAsync(username);
            if (response.IsSuccessStatusCode)
            {
                cache.Set(cacheKey, response.Content, TimeSpan.FromMinutes(5));
                return Results.Ok(response.Content);
            }
            return Results.Problem($"Erro na API GitHub: {response.StatusCode} - {response.Error}");

        })
        .WithOpenApi();

        group.MapGet("/users/{username}/following", async (string username, IGitHubApi api, IMemoryCache cache) =>
        {

            var cacheKey = $"following:{username}";
            if (cache.TryGetValue(cacheKey, out List<GitHubUser>? following))
                return Results.Ok(following);

            var response = await api.GetFollowingAsync(username);
            if (response.IsSuccessStatusCode)
            {
                cache.Set(cacheKey, following, TimeSpan.FromMinutes(5));
                return Results.Ok(following);
            }
            return Results.Problem($"Erro na API GitHub: {response.StatusCode} - {response.Error}");
        })
        .WithOpenApi();

        return group;
    }
}
