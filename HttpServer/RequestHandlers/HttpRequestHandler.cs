using static System.String;

namespace HttpServer.RequestHandlers
{
    public class HttpRequestHandler : IRequestHandler
    {
        public string HandleRequest(string request)
        {
            return Empty;
        }
    }
}