using HttpServer.Handlers;
using HttpServer.RequestHandlers;

namespace HttpServer
{
    public class Router
    {
        private readonly RequestParser _requestParser;
        private IRequestHandler _requestHandler;

        public Router()
        {
            _requestParser = new RequestParser();
        }
        
        internal Response CreateResponse(string requestData)
        {
            var request = _requestParser.Parse(requestData);
            _requestHandler = new DirectoryHandler(request);
            return _requestHandler.CreateResponse();
        }
    }
}