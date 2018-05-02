using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace HttpServer.RequestHandlers
{
    public class Request
    {
        private readonly IOrderedDictionary _headers
            = new OrderedDictionary();

        internal readonly IDictionary<string,string> Parameters 
            = new Dictionary<string, string>();

        public RequestType Type { get; }
        public Version Version { get; }
        public string Resource { get; }
        public string Endpoint { get; }
        public string Path { get; }
        public string Body { get; set; }
        public bool IsEndpoint => !string.IsNullOrEmpty(Endpoint);
        
        internal uint RangeStart { get; set; }
        internal uint? RangeEnd { get; set; }
        
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
            _headers.Add(feild, value);
        }

        internal bool TryGetHeader(string feild, out string value)
        {
            if (!_headers.Contains(feild))
            {
                value = null;
                
                return false;
            }

            value = (string) _headers[feild];
            return true;
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

        public void AddParameter(string field, string value)
        {
            Parameters.TryAdd(field, value);
        }
    }
}