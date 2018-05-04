namespace HttpServer.RequestHandlers
{
    internal class NoAuthorizationScheme : IAuthorizationScheme
    {
        public string Name { get; } = "None";
        public bool IsAuthorized(string token, string username, string password)
        {
            return false;
        }
    }
}