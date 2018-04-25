using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.RequestHandlers.ResponseCodes;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class DirectoryHandler : IRequestHandler
    {
        private const string DefaultDirectory = "wwwroot";

        private readonly Request _request;
        private readonly string _directory;

        public DirectoryHandler(Request request, string publicRoot = null)
        {
            _request = request ?? throw new ArgumentException(nameof(request));
            _directory = publicRoot ?? Path.Combine(Directory.GetCurrentDirectory(), DefaultDirectory);
        }

        public Response CreateResponse()
        {
            var directory = Path.Combine(_directory, _request.Resource.TrimStart(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;

            if (ResourceNotFound(directory))
            {
                return new Response(new NotFound());
            }

            return CreateSuccessResponse(directory);
        }

        private bool ResourceNotFound(string directory)
        {
            return !Directory.Exists(directory);
        }

        private Response CreateSuccessResponse(string directory)
        {
            var response = new Response(new Success());
            
            response.Body = string.Join("<br>", Directory.GetFiles(directory));

            return response;
        }
    }
}