using System;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        public RequestType Type { get; }
        public string Path { get; }
        public Version Version { get; }

        public Request(RequestType type, string path, Version version)
        {
            Type = type;
            Path = path;
            Version = version;
        }
    }
}