using System.Collections.Generic;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

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

        private static Response MethodNotAllowedResponse(Request request) => new Response(HttpStatusCodes.MethodNotAllowed, request);
        private static Response NotFoundResponse(Request request) => new Response(HttpStatusCodes.NotFound, request);
        private static Response BadRequestResponse(Request request) => new Response(HttpStatusCodes.BadRequest, request);

        public Router()
        {
            _handlers.Add((RequestTypes.Unknown, null), new MalformedHandler());
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

            if (request.Type == RequestTypes.Head)
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
                return MethodNotAllowedResponse(request);
            }

            if (request.Type == RequestTypes.Unknown)
            {
                return BadRequestResponse(request);
            }

            return NotFoundResponse(request);
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

            requestTypes.Add(RequestTypes.Options);
            requestTypes.Add(requestType);

            if (requestType == RequestTypes.Get)
            {
                requestTypes.Add(RequestTypes.Head);
            }

            _handlers.TryAdd((RequestTypes.Options, path), new OptionsHandler(requestTypes));
        }

        private Response CreateHeadResponse(Request request)
        {
            if (_handlers.TryGetValue((RequestTypes.Get, request.Resource), out var requestHandler))
            {
                var response = requestHandler.Handle(request);
                response.StringBody = string.Empty;

                return response;
            }

            return NotFoundResponse(request);
        }
    }
}