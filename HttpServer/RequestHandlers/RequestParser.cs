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

            var request = BuildRequest(requestLineSubStrings);
            AddHeadersToRequest(request, lines.Skip(1));

            return request;
        }

        private Request BuildRequest(IReadOnlyList<string> requestLineSubStrings)
        {
            var request = new Request(
                RequestTypeFromString(requestLineSubStrings[0]),
                requestLineSubStrings[1],
                EndpointFrom(requestLineSubStrings[1]),
                HttpVersionFromString(requestLineSubStrings[2])
            );
            
            return request;
        }

        private void AddHeadersToRequest(Request request, IEnumerable<string> headerLines)
        {
            foreach (var headerline in headerLines)
            {
                var headerParts = headerline.Split(':', 2);
                
                if (headerParts.Length != 2) continue;
                
                request.AddHeader(headerParts[0], headerParts[1]);
            }
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
            var split = resource.Split('/');
            return split.Last();
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