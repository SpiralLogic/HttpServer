using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HttpServer.RequestHandlers
{
    internal class RequestParser
    {
        private static Request BadRequest => new Request(RequestTypes.Unknown, string.Empty, string.Empty, new Version());

        public Request Parse(string requestString)
        {
            var requestPieces = requestString.Split("\r\n\r\n", 2);
            var headers = requestPieces.First();

            var headerLines = Regex.Split(headers, "\r\n|\r|\n");
            var requestLineSubStrings = headerLines.First().Split();

            if (requestLineSubStrings.Length != 3)
            {
                return BadRequest;
            }

            var request = CreateRequest(requestLineSubStrings);
            AddHeadersToRequest(request, headerLines.Skip(1));
            AddByteRangesToRequest(request);
            AddBodyToRequest(request, requestPieces);
            AddAuthorizationTo(request);

            return request;
        }

        private static void AddBodyToRequest(Request request, string[] requestSplit)
        {
            if (requestSplit == null || requestSplit.Length != 2)
                return;

            request.Body = requestSplit.Skip(1).Take(1).First();
        }

        private Request CreateRequest(IReadOnlyList<string> requestLineSubStrings)
        {
            var resource = ResourceFromRequestString(requestLineSubStrings[1]);

            var request = new Request(
                RequestTypeFromString(requestLineSubStrings[0]),
                resource,
                EndpointFrom(resource),
                HttpVersionFromString(requestLineSubStrings[2])
            );

            AddParametersTo(request, requestLineSubStrings[1]);

            return request;
        }

        private void AddParametersTo(Request request, string uriResource)
        {
            var uriSplit = uriResource.Split('?', 2);
            if (uriSplit.Length != 2)
            {
                return;
            }

            foreach (var parmeter in uriSplit[1].Split("&"))
            {
                var parmeterSplit = parmeter.Split("=");
                var field = parmeterSplit.First();
                var value = parmeterSplit.Skip(1).FirstOrDefault();

                request.AddParameter(field, Uri.UnescapeDataString(value));
            }
        }

        private string ResourceFromRequestString(string uriResource)
        {
            return uriResource.Split('?').FirstOrDefault();
        }

        private static void AddHeadersToRequest(Request request, IEnumerable<string> headerLines)
        {
            foreach (var headerline in headerLines)
            {
                var headerParts = headerline.Split(':', 2);

                if (headerParts.Length != 2) continue;

                request.AddHeader(headerParts[0], headerParts[1]);
            }
        }

        private static void AddByteRangesToRequest(Request request)
        {
            if (!request.TryGetHeader("Range", out var rangeHeader))
            {
                return;
            }

            var bytesSplit = Regex.Split(rangeHeader, "bytes=([0-9]*)-([0-9]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (int.TryParse(bytesSplit[1], out var rangeStart))
            {
                request.RangeStart = rangeStart;
            }

            if (int.TryParse(bytesSplit[2], out var rangeEnd))
            {
                request.RangeEnd = rangeEnd;
            }
        }

        private void AddAuthorizationTo(Request request)
        {
            var password = string.Empty;
            IAuthorizationScheme authorizationScheme = new NoAuthorizationScheme();

            if (request.TryGetHeader("Authorization", out var authorizationHeader))
            {
                var authorizationHeaderSplit = Regex.Split(authorizationHeader, "([a-z]+) (.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                authorizationScheme = AuthorizationSchemeFactory.FromString(authorizationHeaderSplit[1]);

                if (authorizationHeaderSplit.Length > 2)
                {
                    password = authorizationHeaderSplit[2];
                }
            }

            request.Authorization = new Authorization(authorizationScheme, password);
        }

        private static RequestType RequestTypeFromString(string type)
        {
            switch (type)
            {
                case "GET":
                    return RequestTypes.Get;
                case "HEAD":
                    return RequestTypes.Head;
                case "OPTIONS":
                    return RequestTypes.Options;
                case "POST":
                    return RequestTypes.Post;
                case "PUT":
                    return RequestTypes.Put;
                case "PATCH":
                    return RequestTypes.Patch;
                case "DELETE":
                    return RequestTypes.Delete;
                default:
                    return RequestTypes.Unknown;
            }
        }

        private static string EndpointFrom(string resource)
        {
            var split = resource.Split('/');
            return split.Last();
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