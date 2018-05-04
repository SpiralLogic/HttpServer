using System.IO;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServerCobSpec.Handlers
{
    public class PatchHandler : DirectoryHandler
    {
        private string _etag;

        public PatchHandler(string directory = null) : base(directory)
        {
        }

        public override Response Handle(Request request)
        {
            if (request.Type == RequestTypes.Patch)
            {
                return CreatePatchResponse(request);
            }

            return new Response(HttpStatusCodes.MethodNotAllowed, request);
        }

        private Response CreatePatchResponse(Request request)
        {
            if (request.TryGetHeader("etag", out var etag))
            {
                _etag = etag;
            }

            try
            {
                var file = Path.Combine(_directory, request.Endpoint);
                File.WriteAllText(file, request.Body);
            }
            catch (IOException)
            {
                return new Response(HttpStatusCodes.BadRequest, request);
            }

            return new Response(HttpStatusCodes.NoContent, request);
        }
    }
}