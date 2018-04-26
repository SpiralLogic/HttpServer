using System.Collections.Generic;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer
{
    public class Router
    {
        private readonly RequestParser _requestParser;

        private readonly IDictionary<(RequestType, string), IRequestHandler> _handlers
            = new Dictionary<(RequestType, string), IRequestHandler>();

        public Router()
        {
            _requestParser = new RequestParser();
            _handlers.Add((RequestType.UNKNOWN, null), new MalformedHandler());
        }

        public void AddRoute(RequestType requestType, string path, IRequestHandler requestHandler)
        {
            _handlers.Add((requestType, path), requestHandler);
        }

        internal Response CreateResponse(string requestData)
        {
            var request = _requestParser.Parse(requestData);

            if (_handlers.TryGetValue((request.Type, request.Resource), out var requestHandler))
            {
                return requestHandler.Handle(request);
            }

            return new Response(new NotFound());
        }
    }
}