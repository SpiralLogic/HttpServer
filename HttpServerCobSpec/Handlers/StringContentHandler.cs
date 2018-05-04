using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class StringContentHandler : IRequestHandler
    {
        private readonly string _content;

        public StringContentHandler(string content = null)
        {
            _content = content;
        }

        public Response Handle(Request request)
        {
            var response = new Response(HttpStatusCodes.Ok, request);

            if (!string.IsNullOrEmpty(_content))
            {
                response.StringBody = _content;
            }

            return response;
        }
    }
}