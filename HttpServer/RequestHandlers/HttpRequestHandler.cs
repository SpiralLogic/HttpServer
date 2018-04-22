using System.IO;
using HttpServer.RequestHandlers.ResponseCodes;

namespace HttpServer.RequestHandlers
{
    public class HttpRequestHandler : IRequestHandler
    {
        private HttpRequestParser _requestParser;
        private string _root;

        public HttpRequestHandler(string publicRoot = null)
        {
            _requestParser = new HttpRequestParser();
            _root = publicRoot ?? Directory.GetCurrentDirectory();
        }

        public HttpRequest ParseRequest(string request)
        {
            return _requestParser.Parse(request);
        }

        public string CreateResponse(HttpRequest request)
        {
            if (Directory.Exists(request.Resource))
            {
                return new Response(new Success()).ToString();
            }

            return new Response(new NotFound()).ToString();
        }
    }
}