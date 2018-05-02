using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class BasicAuthHandler : IRequestHandler
    {
        // TODO: Don't hardcode this
        private readonly string _user = "admin";
        private readonly string _password = "password";

        public Response Handle(Request request)
        {
            var response = new Response(new Success(), request);

            return response;
        }
    }
}