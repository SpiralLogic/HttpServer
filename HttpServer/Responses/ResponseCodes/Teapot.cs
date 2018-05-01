namespace HttpServer.Responses.ResponseCodes
{
    internal class Teapot : IHttpStatusCode
    {
        public int Code { get; } = 418;
        public string Status { get; } = "I'm a teapot";
    }
}