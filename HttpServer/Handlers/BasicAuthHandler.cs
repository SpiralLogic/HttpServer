using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class BasicAuthHandler : IRequestHandler
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthHandler(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public Response Handle(Request request)
        {
            Response response;
            if (!request.Authorization.IsAuthorized(_username, _password))
            {
                response = new Response(new NotAuthorized(), request);
            }
            else
            {
                response = new Response(new Success(), request);
                response.StringBody = "GET /log HTTP/1.1\n" +
                                      "PUT /these HTTP/1.1\n" +
                                      "HEAD /requests HTTP/1.1";
            }


            response.AddHeader("WWW-Authenticate", "Basic realm=\"User Visible Realm\"");
            return response;
        }
    }

    public class NotAuthorized : IHttpStatusCode
    {
        public int Code { get; } = 401;
        public string Status { get; } = "Not Authorized";
    }
}