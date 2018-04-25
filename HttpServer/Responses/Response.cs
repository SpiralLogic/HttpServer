using System;
using HttpServer.RequestHandlers.ResponseCodes;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.RequestHandlers
{
    public class Response
    {
        private const string CrLf = "\r\n";
        private readonly Version _version;
        private readonly IHttpStatusCode _statusCode;

        public Response(IHttpStatusCode statusCode, Version version = null)
        {
            _version = version ?? HttpVersion.Version11;
            _statusCode = statusCode;
        }

        public string Body { get; set; }

        public override string ToString()
        {
            return MakeStatusLine() + MakeBody();
        }

        private string MakeStatusLine()
        {
            return $"HTTP/{_version.Major}.{_version.Minor} {_statusCode.Code} {_statusCode.Status} {CrLf}";
        }

        private string MakeBody()
        {
            return CrLf + CrLf + Body;
        }
    }
}