using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Refit;
using RefitGit.Handlers;
using RefitGit.Interfaces;
using RefitGit.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "GitHub Refit API", Version = "v1" });
});

// Polly policies
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

// Logging
builder.Services.AddTransient<LoggingHandler>();

// GitHub Refit client com Polly e LoggingHandler
builder.Services.AddRefitClient<IGitHubApi>()
    .ConfigureHttpClient((sp, c) =>
    {
        c.BaseAddress = new Uri("https://api.github.com");
        c.DefaultRequestHeaders.UserAgent.ParseAdd("Refit-Advanced-App");
        var token = builder.Configuration["GitHub:Token"];
        if (!string.IsNullOrEmpty(token))
            c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    })
    .AddHttpMessageHandler<LoggingHandler>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// GET /users/{username}
app.MapGet("/users/{username}", async (string username, IGitHubApi api, IMemoryCache cache) =>
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

app.MapGet("/users/{username}/repos", async (string username, IGitHubApi api, IMemoryCache cache) =>
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

app.MapGet("/repos/{owner}/{repo}", async (string owner, string repo, IGitHubApi api, IMemoryCache cache) =>
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

app.MapGet("/users/{username}/followers", async (string username, IGitHubApi api, IMemoryCache cache) =>
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

app.MapGet("/users/{username}/following", async (string username, IGitHubApi api, IMemoryCache cache) =>
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


app.Run();
