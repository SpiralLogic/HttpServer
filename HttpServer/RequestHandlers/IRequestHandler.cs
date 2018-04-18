namespace HttpServer
{
    public interface IRequestHandler
    {
        string HandleRequest(string request);
    }
}