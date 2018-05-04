using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    internal class MalformedHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            return new Response(HttpStatusCodes.BadRequest, request);
        }
    }
}