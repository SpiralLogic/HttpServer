using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Responses
{
    public class Response
    {
        private const string CrLf = "\r\n";
        private readonly Version _version;
        private IHttpStatusCode _statusCode;
        private readonly IList<(string feild, string value)> _headers = new List<(string, string)>();
        public Request Request { get; }

        public Response(IHttpStatusCode statusCode, Request request)
        {
            Request = request ?? throw new ArgumentException(nameof(request));
            _version = Request.Version ?? HttpVersion.Version11;
            _statusCode = statusCode;
        }

        public string StringBody { get; set; }
        public byte[] BodyBytes { get; set; }

        public void AddHeader(string feild, string value)
        {
            _headers.Add((feild, value));
        }

        public byte[] Bytes(Encoding encoding)
        {
            var header = encoding.GetBytes(MakeStatusLine() + MakeHeaders() + CrLf);
            var body = RequestedBodyBytes(encoding);

            return CombineByteArrays(header, body);
        }

        private byte[] RequestedBodyBytes(Encoding encoding)
        {
            var bytes = BodyBytes ?? encoding.GetBytes(MakeBody());
            return bytes;
            if (Request.RangeEnd == -1) Request.RangeEnd = bytes.Length;

            var length = Request.RangeEnd - Request.RangeStart;
            var result = new byte[length];
            Array.Copy(bytes, Request.RangeStart, result, 0, length);

            _statusCode = new PartialContent();
            return result;
        }

        private static byte[] CombineByteArrays(byte[] array1, byte[] array2)
        {
            var result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, result, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);

            return result;
        }

        private string MakeHeaders()
        {
            var headers = _headers.Select(header => $"{header.feild}: {header.value}");

            return string.Join(CrLf, headers) + CrLf;
        }

        private string MakeStatusLine()
        {
            return $"HTTP/{_version.Major}.{_version.Minor} {_statusCode.Code} {_statusCode.Status} {CrLf}";
        }

        private string MakeBody()
        {
            if (string.IsNullOrEmpty(StringBody))
            {
                return string.Empty;
            }

            return StringBody;
        }
    }
}