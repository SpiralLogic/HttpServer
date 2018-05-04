using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.RequestHandlers;

namespace HttpServer.Responses
{
    public class Response
    {
        private const string CrLf = "\r\n";
        private readonly Version _version;
        private HttpStatusCode _statusCode;
        private readonly IList<(string feild, string value)> _headers = new List<(string, string)>();
        public Request Request { get; }

        public Response(HttpStatusCode statusCode, Request request)
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
            var body = RequestedBodyBytes(encoding);
            var header = encoding.GetBytes(MakeStatusLine() + MakeHeaders() + CrLf);

            return CombineByteArrays(header, body);
        }

        private byte[] RequestedBodyBytes(Encoding encoding)
        {
            var bytes = BodyBytes ?? encoding.GetBytes(MakeBody());

            if (Request.RangeStart == null && Request.RangeEnd == null)
            {
                return bytes;
            }
        
            if (Request.RangeStart == null && Request.RangeEnd.HasValue)
            {
                Request.RangeStart = bytes.Length - Request.RangeEnd;
                Request.RangeEnd = bytes.Length - 1;
            }

            if (Request.RangeEnd == null)
            {
                Request.RangeEnd = bytes.Length - 1;
            }

            if (Request.RangeEnd > bytes.Length ||
                Request.RangeEnd < Request.RangeStart ||
                Request.RangeEnd < 0 ||
                Request.RangeStart < 0 ||
                Request.RangeStart > bytes.Length)
            {
                AddHeader("Content-Range", $"bytes */{bytes.Length}");
                _statusCode = HttpStatusCodes.RangeNotSatisfiable;

                return new byte[] {0};
            }

            AddHeader("Content-Range", $"bytes {Request.RangeStart}-{Request.RangeEnd}/{bytes.Length}");
            _statusCode = HttpStatusCodes.PartialContent;


            var length = Request.RangeEnd.Value - Request.RangeStart.Value +1;
            
            var result = new byte[length];
            Array.Copy(bytes, Request.RangeStart.Value, result, 0, length);
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