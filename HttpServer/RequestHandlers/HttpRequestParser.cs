﻿using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace HttpServer.RequestHandlers
{
    internal class HttpRequestParser
    {
        public HttpRequest Parse(string requestString)
        {
            var lines = Regex.Split(requestString, "\r\n|\r|\n");
            var firstRequestLine = lines.First().Split();

            var type = RequestTypeFromString(firstRequestLine[0]);
            var resource = firstRequestLine[1];
            var httpVersion = HttpVersionFromString(firstRequestLine[2]);

            return new HttpRequest(type, resource, httpVersion);
        }

        private RequestType RequestTypeFromString(string type)
        {
            switch (type)
            {
                case "GET":
                    return RequestType.GET;
                case "HEAD":
                    return RequestType.HEAD;
                case "OPTION":
                    return RequestType.OPTION;
                case "POST":
                    return RequestType.POST;
                case "PUT":
                    return RequestType.PUT;
                default:
                    return RequestType.UNKNOWN;
            }
        }

        private Version HttpVersionFromString(string type)
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