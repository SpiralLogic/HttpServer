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
        private readonly IHttpStatusCode _statusCode;
        private readonly IList<(string feild, string value)> _headers = new List<(string, string)>();
        public Request Request { get; }

        public Response(IHttpStatusCode statusCode, Version version = null, Request request = null)
        {
            _version = version ?? HttpVersion.Version11;
            _statusCode = statusCode;
            Request = request;
        }

        public string StringBody { get; set; }
        public byte[] BodyBytes { get; set; }
        public uint? RangeStart { get; set; }
        public uint? RangeEnd { get; set; }

        public void AddHeader(string feild, string value)
        {
            _headers.Add((feild, value));
        }

        public byte[] Bytes(Encoding encoding)
        {
            var header = encoding.GetBytes(MakeStatusLine() + MakeHeaders() + CrLf);
            var body = BodyBytes ?? encoding.GetBytes(MakeBody());

            return CombineByteArrays(header, body);
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