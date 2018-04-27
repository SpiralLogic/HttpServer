using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
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

        internal Response CreateResponse(string requestData)
        {
            var request = _requestParser.Parse(requestData);

            if (request.Type == RequestType.HEAD)
            {
                return CreateHeadResponse(request);
            }

            if (request.Type == RequestType.UNKNOWN)
            {
                return BadRequestResponse;
            }

            if (_handlers.TryGetValue((request.Type, request.Path), out var requestHandler))
            {
                return requestHandler.Handle(request);
            }

            return NotFoundResponse;
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
            if (_handlers.TryGetValue((RequestType.GET, request.Path), out var requestHandler))
            {
                var response = requestHandler.Handle(request);
                response.Body = string.Empty;

                return response;
            }

            return NotFoundResponse;
        }
    }
}