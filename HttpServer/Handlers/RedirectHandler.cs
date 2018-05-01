using System;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class RedirectHandler : IRequestHandler
    {
        private readonly string _location;

        public RedirectHandler(string location)
        {
            if (string.IsNullOrEmpty(location)) throw new ArgumentException(nameof(location));
            
            _location = location;
        }
        
        public Response Handle(Request request)
        {
            var response = new Response(new Found());
            response.AddHeader("Location", _location);

            return response;
        }
    }
}