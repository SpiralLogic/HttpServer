using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class FormHandler : IRequestHandler
    {
        public FormHandler()
        {
        }

        public Response Handle(Request request)
        {
            return new Response(new Success());
        }
    }
}