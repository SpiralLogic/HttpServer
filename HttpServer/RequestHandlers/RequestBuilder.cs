using System;

namespace HttpServer.RequestHandlers
{
    internal class RequestBuilder
    {
        internal RequestType Type { get; set; }
        internal Version HttpVersion { get; set; }
        internal string Resource { get; set; }
        internal string Endpoint { get; set; }

        internal Request Request => new Request(Type, Resource, Endpoint, HttpVersion);
    }
}