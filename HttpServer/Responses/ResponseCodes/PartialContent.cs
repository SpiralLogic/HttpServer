
namespace HttpServer.Responses.ResponseCodes
{
    internal class PartialContent : IHttpStatusCode
    {
        public int Code { get; } = 206;
        public string Status { get; } = "Partial Content";
    }
}