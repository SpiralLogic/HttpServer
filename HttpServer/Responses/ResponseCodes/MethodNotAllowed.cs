namespace HttpServer.Responses.ResponseCodes
{
    internal class MethodNotAllowed : IHttpStatusCode
    {
        public int Code { get; } = 405;
        public string Status { get; } = "Method Not Allowed";
    }
}