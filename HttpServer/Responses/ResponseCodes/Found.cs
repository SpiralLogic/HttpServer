namespace HttpServer.Responses.ResponseCodes
{
    internal class Found : IHttpStatusCode
    {
        public int Code { get; } = 302;
        public string Status { get; } = "Found";
    }
}