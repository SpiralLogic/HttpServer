namespace HttpServer.RequestHandlers
{
    public class Authorization
    {
        internal Authorization(IAuthorizationScheme scheme, string token)
        {
            Scheme = scheme;
            Token = token;
        }

        public string Token { get; set; }

        internal IAuthorizationScheme Scheme { get; }

        public bool IsAuthorized(string username, string password)
        {
            return Scheme.IsAuthorized(Token, username, password);
        }
    }
}