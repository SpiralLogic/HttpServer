using System;
using System.Text;

namespace HttpServer.RequestHandlers
{
    internal class BasicAuthenticationScheme : IAuthorizationScheme
    {
        public string Name { get; } = "Basic";

        public bool IsAuthorized(string token, string username, string password)
        {

            var plainTextBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(plainTextBytes) == token;
        }
    }
}