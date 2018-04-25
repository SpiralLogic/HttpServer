using HttpServer.Responses.ResponseCodes;

namespace HttpServer.RequestHandlers.ResponseCodes
{
    internal class Success : IHttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "OK";
    }
}