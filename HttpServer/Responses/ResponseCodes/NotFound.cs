namespace HttpServer.Responses.ResponseCodes
{
    public class NotFound : IHttpStatusCode
    {
        public int Code { get; } = 404;
        public string Status { get; } = "Not Found";
    }
}