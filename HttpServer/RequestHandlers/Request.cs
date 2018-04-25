using System;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        public RequestType Type { get; }
        public string Resource { get; }
        public Version Version { get; }

        public Request(RequestType type, string resource, Version version)
        {
            Type = type;
            Resource = resource;
            Version = version;
        }
    }
}