using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Refit;
using RefitGit;
using RefitGit.Handlers;
using RefitGit.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
// Habilita descoberta de endpoints para OpenAPI nativo
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o => o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddOpenApi("v1");

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("X-Correlation-ID"); // Exemplo de header a propagar
});


builder.Services.AddTransient<LoggingHandler>();

var token = builder.Configuration["GitHub:Token"];

// Criando o pipeline de resiliência com Polly v8+
builder.Services.AddResiliencePipeline("GitHubApiPipeline", pipelineBuilder =>
{
    pipelineBuilder
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2),
            ShouldHandle = new PredicateBuilder()
                .Handle<HttpRequestException>()
        })
        .AddTimeout(TimeSpan.FromSeconds(10))
        .AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            MinimumThroughput = 2,
            SamplingDuration = TimeSpan.FromSeconds(30),
            BreakDuration = TimeSpan.FromSeconds(30),
            ShouldHandle = new PredicateBuilder()
                .Handle<HttpRequestException>()
        });
});

builder.Services.AddRefitClient<IGitHubApi>()
    .ConfigureHttpClient((sp, c) =>
    {
        c.BaseAddress = new Uri("https://api.github.com");
        c.DefaultRequestHeaders.UserAgent.ParseAdd("Refit-Advanced-App");
        c.Timeout = TimeSpan.FromSeconds(30);
        if (!string.IsNullOrEmpty(token))
            c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    })
    .AddHeaderPropagation()
    .AddHttpMessageHandler<LoggingHandler>()
    .AddResilienceHandler("GitHubApiPipeline", options =>
    {
        options.Name = "GitHubApiPipeline";
    });

var app = builder.Build();
app.UseHeaderPropagation();
app.MapGroup("/v1").WithGroupName("v1").MapEndpoints();
app.MapOpenApi();

app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";

    c.SwaggerEndpoint("/openapi/v1.json", "RefitGit OpenAPI Docs V1");

    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.None);
    c.EnableDeepLinking();
    c.ShowExtensions();
    c.ShowCommonExtensions();
});


app.Run();
