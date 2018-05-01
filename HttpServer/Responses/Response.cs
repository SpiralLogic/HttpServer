using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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

        public Response(IHttpStatusCode statusCode, Version version = null, Request request = null)
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

        public byte[] Bytes(Encoding encoding)
        {
            var header = encoding.GetBytes(MakeStatusLine() + MakeHeaders());
            var body = encoding.GetBytes(MakeBody());

            var response = new byte[header.Length + body.Length];
            Array.Copy(header, response, header.Length);
            Array.Copy(body, 0, response, header.Length, body.Length);
            
            return response;
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