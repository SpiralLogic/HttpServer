using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class DirectoryHandler : IRequestHandler
    {
        private const string DefaultDirectory = "wwwroot";
        private readonly string _directory;

        public DirectoryHandler(string publicRoot = null)
        {
            _directory = publicRoot ?? Path.Combine(Directory.GetCurrentDirectory(), DefaultDirectory);
        }

        public Response Handle(Request request)
        {
            request = request ?? throw new ArgumentException(nameof(request));
            var directory = Path.Combine(_directory, request.Resource.TrimStart(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;

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