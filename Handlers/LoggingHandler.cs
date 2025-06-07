namespace RefitGit.Handlers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending request to {Url}", request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation("Response {StatusCode} from {Url}", response.StatusCode, request.RequestUri);

            return response;
        }
    }
}
