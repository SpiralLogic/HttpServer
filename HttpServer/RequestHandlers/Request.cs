using System;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        public RequestType Type { get; } = RequestType.UNKNOWN;
        public Version Version { get; }
        public string Resource { get; }
        public string Endpoint { get; }
        //public string Resource => Path + Endpoint;
        public bool IsEndpoint => string.IsNullOrEmpty(Endpoint);
    
        public Request(RequestType type, string resource, string endpoint, Version version)
        {
            Type = type;
            Resource = resource;
            Version = version;
            Endpoint = endpoint;
        }

        public override string ToString()
        {
            return $"Path: {Resource} Endpoint: {Endpoint} Resource:{Resource}";
        }
    }
}