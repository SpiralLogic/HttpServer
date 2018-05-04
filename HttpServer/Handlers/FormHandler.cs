using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class FormHandler : IRequestHandler
    {

        public Response Handle(Request request)
        {
            return new Response(HttpStatusCodes.Ok, request);
        }
    }
}