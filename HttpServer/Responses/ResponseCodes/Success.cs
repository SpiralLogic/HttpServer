namespace HttpServer.Responses.ResponseCodes
{
    internal class Success : IHttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "OK";
    }
}