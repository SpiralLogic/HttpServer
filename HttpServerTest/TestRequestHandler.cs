using System;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses.ResponseCodes;

namespace HttpServerTest
{
    internal class TestRequestHandler : IRequestHandler
    {
        internal event EventHandler RequestRecievedEvent;
        internal Request LastRequest;

        public Response Handle(Request request)
        {
            LastRequest = request;
            RequestRecievedEvent?.Invoke(this, new EventArgs());
        
            return new Response(new TestStatusCode());
        }
    }

    internal class TestRequest : Request
    {
        public TestRequest() : base(RequestType.GET, "/", HttpVersion.Version11)
        {
        }
    }

    internal class TestStatusCode : IHttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "yes";
    }
}