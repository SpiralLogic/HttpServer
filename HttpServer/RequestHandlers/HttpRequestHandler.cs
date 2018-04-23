using System;
using System.IO;
using HttpServer.Loggers;
using HttpServer.RequestHandlers.ResponseCodes;

namespace HttpServer.RequestHandlers
{
    public class HttpRequestHandler : IRequestHandler
    {
        private readonly HttpRequestParser _requestParser;
        private string _root;
        private ILogger _logger;
        private const string DefaultPublicRoot = "wwwroot";

        public HttpRequestHandler(ILogger logger, string publicRoot = null)
        {
            _requestParser = new HttpRequestParser();
            _root = publicRoot ?? Path.Combine(Directory.GetCurrentDirectory(), DefaultPublicRoot);
            _logger = logger;
        }

        public HttpRequest ParseRequest(string request)
        {
            return _requestParser.Parse(request);
        }

        public Response CreateResponse(HttpRequest request)
        {
            var directory = Path.Combine(_root, request.Resource.TrimStart(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;
            var filename = Path.GetFileName(directory);

            if (ResourceNotFound(directory, filename))
            {
                return new Response(new NotFound());
            }

            return CreateSuccessResponse(directory, filename);
        }

        private bool ResourceNotFound(string directory, string filename)
        {
            return !Directory.Exists(directory) || Directory.Exists(directory) && !string.IsNullOrEmpty(filename) && !File.Exists(filename);
        }

        private Response CreateSuccessResponse(string directory, string filename)
        {
            var response = new Response(new Success());
            if (Directory.Exists(directory) && string.IsNullOrEmpty(filename))
            {
                response.Body = string.Join("<br>", Directory.GetFiles(directory));
            }

            return response;
        }
    }
}