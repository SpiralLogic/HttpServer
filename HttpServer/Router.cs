using System.Collections.Generic;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer
{
    public class Router
    {
        private readonly RequestParser _requestParser = new RequestParser();

        private readonly IDictionary<(RequestType, string), IRequestHandler> _handlers
            = new Dictionary<(RequestType, string), IRequestHandler>();

        private readonly IDictionary<string, IList<RequestType>> _pathOptions
            = new Dictionary<string, IList<RequestType>>();

        private readonly ISet<string> _directoryRoutes = new HashSet<string>();
        private static Response MethodNotAllowedResponse => new Response(new MethodNotAllowed());

        private static Response NotFoundResponse => new Response(new NotFound());
        private static Response BadRequestResponse => new Response(new BadRequest());

        public Router()
        {
            _handlers.Add((RequestType.UNKNOWN, null), new MalformedHandler());
        }

        public void AddRoute(RequestType requestType, string path, IRequestHandler requestHandler)
        {
            _handlers.Add((requestType, path), requestHandler);
            AddPathOptions(requestType, path);
        }

        public void AddDirectoryRoute(RequestType requestType, string path, IRequestHandler requestHandler)
        {
            _directoryRoutes.Add(path);
            AddRoute(requestType, path, requestHandler);
        }

        internal Response CreateResponse(string requestData)
        {
            var request = _requestParser.Parse(requestData);

            if (request.Type == RequestType.HEAD)
            {
                return CreateHeadResponse(request);
            }


            if (HasRequestHandler(request, out var requestHandler))
            {
                return requestHandler.Handle(request);
            }

            if (HadDirectoryHandler(request, out var directoryRequestHandler))
            {
                return directoryRequestHandler.Handle(request);
            }

            if (MethodNotAllowed(request))
            {
                return MethodNotAllowedResponse;
            }

            if (request.Type == RequestType.UNKNOWN)
            {
                return BadRequestResponse;
            }

            return NotFoundResponse;
        }

        private bool MethodNotAllowed(Request request)
        {
            return !_handlers.ContainsKey((request.Type, request.Resource)) && !_handlers.ContainsKey((request.Type, request.Path));
        }

        private bool HasRequestHandler(Request request, out IRequestHandler requestHandler)
        {
            return _handlers.TryGetValue((request.Type, request.Resource), out requestHandler);
        }

        private bool HadDirectoryHandler(Request request, out IRequestHandler directoryRequestHandler)
        {
            return _handlers.TryGetValue((request.Type, request.Path), out directoryRequestHandler) && _directoryRoutes.Contains(request.Path);
        }

        private void AddPathOptions(RequestType requestType, string path)
        {
            if (!_pathOptions.TryGetValue(path, out var requestTypes))
            {
                requestTypes = new List<RequestType>();
                _pathOptions.Add(path, requestTypes);
            }

            requestTypes.Add(RequestType.OPTIONS);
            requestTypes.Add(requestType);

            if (requestType == RequestType.GET)
            {
                requestTypes.Add(RequestType.HEAD);
            }

            _handlers.TryAdd((RequestType.OPTIONS, path), new OptionsHandler(requestTypes));
        }

        private Response CreateHeadResponse(Request request)
        {
            if (_handlers.TryGetValue((RequestType.GET, request.Resource), out var requestHandler))
            {
                var response = requestHandler.Handle(request);
                response.StringBody = string.Empty;

                return response;
            }

            return NotFoundResponse;
        }
    }
}