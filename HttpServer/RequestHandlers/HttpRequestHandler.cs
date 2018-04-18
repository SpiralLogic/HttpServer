using HttpServer.Loggers;
using static System.String;

namespace HttpServer.RequestHandlers
{
    public class HttpRequestHandler : IRequestHandler
    {
        public HttpRequestHandler()
        {
        }

        public string HandleRequest(string request)
        {
            return Empty;
        }
    }
}