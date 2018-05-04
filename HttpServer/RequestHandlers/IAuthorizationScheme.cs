namespace HttpServer.RequestHandlers
{
    internal interface IAuthorizationScheme
    {
        string Name { get; }
        bool IsAuthorized(string token, string username, string password);
    }
}