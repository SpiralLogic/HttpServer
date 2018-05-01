using System;
using System.Collections.Generic;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        private readonly ICollection<(string feild, string value)> _headers = new List<(string feild, string value)>();
        
        public RequestType Type { get; }
        public Version Version { get; }
        public string Resource { get; }
        public string Endpoint { get; }
        public string Path { get; }
        public bool IsEndpoint => !string.IsNullOrEmpty(Endpoint);

        public Request(RequestType type, string resource, string endpoint, Version version)
        {
            Type = type;
            Resource = resource;
            Endpoint = endpoint;
            Version = version;
            Path = MakePathFrom(resource);

        }

        internal void AddHeader(string feild, string value)
        {
            _headers.Add((feild, value));
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

            return Resource.Remove(Resource.LastIndexOf(Endpoint, StringComparison.Ordinal));
        }
    }
}