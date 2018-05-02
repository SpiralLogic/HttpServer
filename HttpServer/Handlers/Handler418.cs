using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class Handler418 : IRequestHandler
    {
        public Response Handle(Request request)
        {
            return new Response(new Teapot(), request) {StringBody = "I'm a teapot"};
        }
    }
}