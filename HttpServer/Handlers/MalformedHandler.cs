using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    internal class MalformedHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            return new Response(new BadRequest());
        }
    }
}