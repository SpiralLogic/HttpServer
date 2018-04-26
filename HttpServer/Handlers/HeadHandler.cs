using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    internal class HeadHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            return new Response(new BadRequest());
        }

        public Response Handle(Response response)
        {
            return new Response(response);
        }
    }
}