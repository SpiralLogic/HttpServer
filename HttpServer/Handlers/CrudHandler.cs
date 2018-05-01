using HttpServer.Loggers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
using HttpServer.Responses.ResponseCodes;

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
                return RespondWithData();
            }

            if (request.Type == RequestType.POST)
            {
                return RespondWithDataCreated(request.Body);
            }

            if (request.Type == RequestType.PUT)
            {
                return RespondWithDataUpdateSuccess(request.Body);
            }

            if (request.Type == RequestType.DELETE)
            {
                return RespondWithDataDeleteSuccess();
            }

            return new Response(new MethodNotAllowed());
        }

        private Response RespondWithData()
        {
            if (string.IsNullOrEmpty(_data))
                return new Response(new NotFound());

            return new Response(new Success()) {StringBody = _data};
        }

        private Response RespondWithDataCreated(string data)
        {
            _data = data;

            var response = new Response(new Created()) {StringBody = _data};

            response.AddHeader("Location", _dataLocation);

            return response;
        }

        private Response RespondWithDataUpdateSuccess(string data)
        {
            _data = data;

            return new Response(new Success()) {StringBody = _data};
        }

        private Response RespondWithDataDeleteSuccess()
        {
            _data = null;

            return new Response(new Success());
        }
    }
}