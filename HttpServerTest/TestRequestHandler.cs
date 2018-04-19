using HttpServer.RequestHandlers;

namespace HttpServerTest
{
    internal class TestRequestHandler : IRequestHandler
    {
        public string HandleRequest(string request)
        {
            return request;
        }
    }
}
