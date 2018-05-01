namespace HttpServer.Responses.ResponseCodes
{
    internal class NoContent : IHttpStatusCode
    {
        public int Code { get; } = 204;
        public string Status { get; } = "No Content";
    }
}