using System.Linq;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

namespace HttpServer.Handlers
{
    public class ParameterPrintHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var response = new Response(new Success(), request);
            response.StringBody = string.Join("\r\n", request.Parameters.Select(parameter => $"{parameter.Key} = {parameter.Value}"));
            return response;
        }
    }
}