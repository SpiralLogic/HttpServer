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
            if (request.Type == RequestTypes.Get)
            {
                return RespondWithData(request);
            }

            if (request.Type == RequestTypes.Post)
            {
                return RespondWithDataCreated(request);
            }

            if (request.Type == RequestTypes.Put)
            {
                return RespondWithDataUpdateSuccess(request);
            }

            if (request.Type == RequestTypes.Delete)
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