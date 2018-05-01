namespace HttpServer.Responses.ResponseCodes
{
    internal class Created : IHttpStatusCode
    {
        public int Code { get; } = 201;
        public string Status { get; } = "Created";
    }
}