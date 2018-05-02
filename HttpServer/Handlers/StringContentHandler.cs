using System;
using System.IO;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class StringContentHandler : IRequestHandler
    {
        private readonly string _content;

        public StringContentHandler(string content = null)
        {
            _content = content;
        }

        public Response Handle(Request request)
        {
            var response = new Response(new Success(), request);

            if (!string.IsNullOrEmpty(_content))
            {
                response.StringBody = _content;
            }

            return response;
        }
    }
}