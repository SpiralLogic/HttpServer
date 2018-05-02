using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
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

            var directory = Path.Combine(_directory, request.Path.TrimStart('/'));
            var filename = Path.Combine(directory, request.Endpoint);

            if (ResourceNotFound(directory, filename))
            {
                return new Response(new NotFound(), request);
            }

            return CreateSuccessResponse(directory, filename, request);
        }

        private bool ResourceNotFound(string directory, string filename)
        {
            return !Directory.Exists(directory) || Directory.Exists(directory) && !string.IsNullOrEmpty(filename) && !File.Exists(filename);
        }

        private Response CreateSuccessResponse(string directory, string filename, Request request)
        {
            try
            {
                var response = new Response(new Success(), request);
                response.AddHeader("Content-Type", MediaTypeMapper.MediaTypeFromFile(filename));
                response.BodyBytes = File.ReadAllBytes(Path.Combine(directory, filename));

                return response;
            }
            catch (IOException)
            {
                return new Response(new NotImplemented(), request);
            }
        }
    }
}