using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public interface IRequestHandler
    {
        Response Handle(Request request);
    }
}