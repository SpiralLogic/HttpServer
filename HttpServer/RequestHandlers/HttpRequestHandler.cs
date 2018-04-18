using System;
using HttpServer.Loggers;
using static System.String;

namespace HttpServer.RequestHandlers
{
    internal class HttpRequestHandler : IRequestHandler
    {
        private HttpRequestParser _requestParser;
        private readonly ILogger _logger;

        public HttpRequestHandler(HttpRequestParser requestParser, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _requestParser = requestParser ?? throw new ArgumentException(nameof(requestParser));
        }

        public string HandleRequest(string request)
        {
            _logger.Log(request);
            return Empty;
        }
    }

    internal class HttpRequestParser
    {
    }
}