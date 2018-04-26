namespace HttpServer.Responses.ResponseCodes
{
    internal class BadRequest : IHttpStatusCode
    {
        public int Code { get; } = 400;
        public string Status { get; } = "Bad Request";
    }
}