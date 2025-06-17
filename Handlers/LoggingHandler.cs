namespace RefitGit.Handlers;
public class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending request to {Url}", request.RequestUri);

        var response = await base.SendAsync(request, cancellationToken);

        logger.LogInformation("Response {StatusCode} from {Url}", response.StatusCode, request.RequestUri);

        return response;
    }
}
