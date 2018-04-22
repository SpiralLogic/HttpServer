namespace HttpServer.RequestHandlers.ResponseCodes
{
    public interface IHttpStatusCode
    {
        int Code { get; }
        string Status { get; }
    }
}