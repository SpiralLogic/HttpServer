using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.RequestHandlers.ResponseCodes;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class FileRequestHandler : IRequestHandler
    {
        private readonly string _directory;
        private readonly Request _request;
        private const string DefaultDirectory = "wwwroot";

        public FileRequestHandler(Request request, string directory = null)
        {
            _request = request ?? throw new ArgumentException(nameof(request));
            _directory = directory ?? Path.Combine(Directory.GetCurrentDirectory(), DefaultDirectory);
        }

        public Response CreateResponse()
        {
            var directory = Path.Combine(_directory, _request.Resource.TrimStart(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;
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
            try
            {
                response.Body = File.ReadAllText(Path.Combine(directory, filename));

            }
            catch (IOException)
            {
             return new Response(new NotImplemented());   
            }
            return response;
        }
    }
}