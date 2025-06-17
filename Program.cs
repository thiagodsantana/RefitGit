using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Refit;
using RefitGit;
using RefitGit.Handlers;
using RefitGit.Interfaces;
using RefitGit.Models;
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

// Polly policies
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

// Logging
builder.Services.AddTransient<LoggingHandler>();
var token = builder.Configuration["GitHub:Token"];
// GitHub Refit client com Polly e LoggingHandler
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
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();
app.UseHeaderPropagation();
// Mapeia os grupos de endpoints (v1 e v2)
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
