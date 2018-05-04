using System;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;

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
        
            return new Response(new TestStatusCode(200, "Ok"), request);
        }
    }

    internal class TestRequest : Request
    {
        public TestRequest() : base(RequestTypes.Get, "/", string.Empty, HttpVersion.Version11)
        {
        }
    }

    internal class TestStatusCode : HttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "yes";

        protected internal TestStatusCode(int code, string status) : base(code, status)
        {
        }
    }
}