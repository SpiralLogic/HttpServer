namespace HttpServer.Responses.ResponseCodes
{
    public class NotImplemented : IHttpStatusCode
    {
        public int Code { get; } = 500;
        public string Status { get; } = "Not Implemented";
    }
}