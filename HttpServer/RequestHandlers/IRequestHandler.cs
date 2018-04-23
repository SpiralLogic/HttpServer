using System;

namespace HttpServer.RequestHandlers
{
    public interface IRequestHandler
    {
        HttpRequest ParseRequest(string request);

        Response CreateResponse(HttpRequest request);
    }
}