using System;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        public RequestType Type { get; }
        public Version Version { get; }
        public string Resource { get; }
        public string Endpoint { get; }
        public string Path { get; }
        public bool IsEndpoint => string.IsNullOrEmpty(Endpoint);

        public Request(RequestType type, string resource, string endpoint, Version version)
        {
            Type = type;
            Resource = resource;
            Path = MakePathFrom(resource);
            Version = version;
            Endpoint = endpoint;
        }

        public override string ToString()
        {
            return $"Path: {Path} Endpoint: {Endpoint} Resource:{Resource}";
        }

        private string MakePathFrom(string resource)
        {
            if (string.IsNullOrEmpty(Endpoint))
            {
                return resource + "/";
            }

            return Resource.Remove(Resource.LastIndexOf(Endpoint, StringComparison.Ordinal)) + "/";
        }
    }
}