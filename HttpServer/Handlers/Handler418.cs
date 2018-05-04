using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class Handler418 : IRequestHandler
    {
        public Response Handle(Request request)
        {
            return new Response(HttpStatusCodes.Teapot, request) {StringBody = "I'm a teapot"};
        }
    }
}