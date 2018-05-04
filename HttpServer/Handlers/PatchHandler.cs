﻿using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class PatchHandler : IRequestHandler
    {
        private string _etag;
        private readonly string _directory;

        public PatchHandler(string directory = null)
        {
            _directory = directory ?? Path.Combine(Directory.GetCurrentDirectory(), Constants.DefaultDirectory);
        }

        public Response Handle(Request request)
        {
            if (request.Type == RequestType.PATCH)
            {
                return CreatePatchResponse(request);
            }

            return new Response(new MethodNotAllowed(), request);
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
                return new Response(new BadRequest(), request);
            }

            return new Response(new NoContent(), request);
        }
    }
}