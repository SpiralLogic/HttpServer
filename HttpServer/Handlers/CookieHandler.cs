﻿using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class CookieHandler : IRequestHandler
    {
        // TODO: Don't hardcode this
        private readonly string _content;

        public CookieHandler(string content = null)
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

            request.Parameters.TryGetValue("type", out var type);
            
            response.AddHeader("Set-Cookie", $"type={type}");
            return response;
        }
    }
}