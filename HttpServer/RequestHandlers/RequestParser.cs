using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HttpServer.RequestHandlers
{
    internal class RequestParser
    {
        private Request BadRequest => new Request(RequestType.UNKNOWN, string.Empty, string.Empty, new Version());
        
        public Request Parse(string requestString)
        {
            var lines = Regex.Split(requestString, "\r\n|\r|\n");
            var requestLineSubStrings = lines.First().Split();

            if (requestLineSubStrings.Length != 3)
            {
                return BadRequest;
            }
            
            return BuildRequest(requestLineSubStrings);
        }

        private Request BuildRequest(IReadOnlyList<string> requestLineSubStrings)
        {
            var requestBuilder = new RequestBuilder
            {
                Type = RequestTypeFromString(requestLineSubStrings[0]),
                Resource = requestLineSubStrings[1],
            Endpoint = EndpointFrom(requestLineSubStrings[1]),
                HttpVersion = HttpVersionFromString(requestLineSubStrings[2])
            };

            return requestBuilder.Request;
        }

        private RequestType RequestTypeFromString(string type)
        {
            switch (type)
            {
                case "GET":
                    return RequestType.GET;
                case "HEAD":
                    return RequestType.HEAD;
                case "OPTIONS":
                    return RequestType.OPTIONS;
                case "POST":
                    return RequestType.POST;
                case "PUT":
                    return RequestType.PUT;
                default:
                    return RequestType.UNKNOWN;
            }
        }

        private static string EndpointFrom(string resource)
        {
            return resource.Split('/').Last();
        }
        
        private static string CreatePath(string resource, string endpoint)
        {
            return "/";
        }

        private static Version HttpVersionFromString(string type)
        {
            switch (type)
            {
                case "HTTP/1.1":
                    return HttpVersion.Version11;
                case "HTTP/1.0":
                    return HttpVersion.Version10;
                case "HTTP/2.0":
                    return HttpVersion.Version10;
                default:
                    return HttpVersion.Unknown;
            }
        }
    }
}