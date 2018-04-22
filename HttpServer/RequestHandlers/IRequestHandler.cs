using System;

namespace HttpServer.RequestHandlers
{
    public interface IRequestHandler
    {
        HttpRequest ParseRequest(string request);

        string CreateResponse(HttpRequest request);
    }
}