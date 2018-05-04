using HttpServer.Loggers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

namespace HttpServer.Handlers
{
    public class CrudHandler : IRequestHandler
    {
        private string _data;
        private readonly string _dataLocation;

        public CrudHandler(string dataLocation)
        {
            _dataLocation = dataLocation;
        }

        public Response Handle(Request request)
        {
            if (request.Type == RequestType.GET)
            {
                return RespondWithData(request);
            }

            if (request.Type == RequestType.POST)
            {
                return RespondWithDataCreated(request);
            }

            if (request.Type == RequestType.PUT)
            {
                return RespondWithDataUpdateSuccess(request);
            }

            if (request.Type == RequestType.DELETE)
            {
                return RespondWithDataDeleteSuccess(request);
            }

            return new Response(HttpStatusCodes.MethodNotAllowed, request);
        }

        private Response RespondWithData(Request request)
        {
            if (string.IsNullOrEmpty(_data))
                return new Response(HttpStatusCodes.NotFound,request);

            return new Response(HttpStatusCodes.Ok, request) {StringBody = _data};
        }

        private Response RespondWithDataCreated(Request request)
        {
            _data = request.Body;

            var response = new Response(HttpStatusCodes.Created, request) {StringBody = _data};

            response.AddHeader("Location", _dataLocation);

            return response;
        }

        private Response RespondWithDataUpdateSuccess(Request request)
        {
            _data = request.Body;

            return new Response(HttpStatusCodes.Ok, request) {StringBody = _data};
        }

        private Response RespondWithDataDeleteSuccess(Request request)
        {
            _data = null;

            return new Response(HttpStatusCodes.Ok, request);
        }
    }
}