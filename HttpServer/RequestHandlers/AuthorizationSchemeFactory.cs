namespace HttpServer.RequestHandlers
{
    public static class AuthorizationSchemeFactory
    {
        
        internal static IAuthorizationScheme FromString(string schemeName)
        {
            if (schemeName == "Basic")
            {
                return new BasicAuthenticationScheme();
            }

            return new NoAuthorizationScheme();
        }
    }
}