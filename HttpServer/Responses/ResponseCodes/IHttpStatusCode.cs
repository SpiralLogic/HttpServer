namespace HttpServer.Responses.ResponseCodes
{
    public interface IHttpStatusCode
    {
        int Code { get; }
        string Status { get; }
    }
}