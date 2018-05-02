using System.Text;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class PartialContentTest : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var response = new Response(new PartialContent(), request);

                response.BodyBytes = Encoding.ASCII.GetBytes("This");
            
            return response;
        }
    }
}