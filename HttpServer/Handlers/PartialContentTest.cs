using System.Text;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class PartialContentTest : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var response = new Response(HttpStatusCodes.PartialContent, request);

            response.StringBody = $"{request.RangeStart} {request.RangeEnd} {request.RangeEnd - request.RangeStart + 1}";

            return response;
        }
    }
}