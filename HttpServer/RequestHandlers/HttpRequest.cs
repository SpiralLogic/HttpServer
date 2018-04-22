using System;

namespace HttpServer.RequestHandlers
{
    public class HttpRequest
    {
        public RequestType Type { get; }
        public string Resource { get; }
        public Version Version { get; }

        public HttpRequest(RequestType type, string resource, Version version)
        {
            Type = type;
            Resource = resource;
            Version = version;
        }
    }
}