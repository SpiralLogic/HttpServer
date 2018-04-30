using System;
using System.Collections.Generic;
using System.Linq;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Responses
{
    public class Response
    {
        private const string CrLf = "\r\n";
        private readonly Version _version;
        private readonly IHttpStatusCode _statusCode;
        private readonly IList<(string feild, string value)> _headers = new List<(string, string)>();
        public Request Request { get; }

        public Response(IHttpStatusCode statusCode, Version version = null, Request request=null)
        {
            _version = version ?? HttpVersion.Version11;
            _statusCode = statusCode;
            Request = request;
        }

        public string Body { get; set; }

        public void AddHeader(string feild, string value)
        {
            _headers.Add((feild, value));
        }

        public override string ToString()
        {
            return MakeStatusLine()
                   + MakeHeaders()
                   + MakeBody();
        }

        private string MakeHeaders()
        {
            var headers = _headers.Select(header => $"{header.feild}: {header.value}");

            return string.Join(CrLf, headers);
        }

        private string MakeStatusLine()
        {
            return $"HTTP/{_version.Major}.{_version.Minor} {_statusCode.Code} {_statusCode.Status} {CrLf}";
        }

        private string MakeBody()
        {
            if (string.IsNullOrEmpty(Body))
            {
                return CrLf;
            }

            return CrLf + Body;
        }
    }
}