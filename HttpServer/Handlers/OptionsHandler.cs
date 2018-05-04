using System;
using System.Collections.Generic;
using System.Linq;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class OptionsHandler : IRequestHandler
    {
        private readonly IEnumerable<RequestType> _requestTypes;

        internal OptionsHandler(IEnumerable<RequestType> requestTypes)
        {
            _requestTypes = requestTypes;
        }

        public Response Handle(Request request)
        {
            var response = new Response(HttpStatusCodes.Ok, request);
            response.AddHeader("Allow", CreateAllowHeader());

            return response;
        }

        private string CreateAllowHeader()
        {
            var allowedRequestTypes = _requestTypes.Select(type => type.Name);

            return string.Join(", ", allowedRequestTypes);
        }
    }
}