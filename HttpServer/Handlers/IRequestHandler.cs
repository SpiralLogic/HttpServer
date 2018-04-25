using HttpServer.RequestHandlers;

namespace HttpServer.Handlers
{
    public interface IRequestHandler
    {
        Response CreateResponse();
    }
}