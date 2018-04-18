namespace HttpServer.RequestHandlers
{
    public interface IRequestHandler
    {
        string HandleRequest(string request);
    }
}