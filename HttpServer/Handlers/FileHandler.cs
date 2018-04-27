using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class FileHandler : IRequestHandler
    {
        private readonly string _directory;
        private const string DefaultDirectory = "wwwroot";

        public FileHandler(string directory = null)
        {
            _directory = directory ?? Path.Combine(Directory.GetCurrentDirectory(), DefaultDirectory);
        }

        public Response Handle(Request request)
        {
            request = request ?? throw new ArgumentException(nameof(request));
            var directory = Path.Combine(_directory, request.Path.TrimStart(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;
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