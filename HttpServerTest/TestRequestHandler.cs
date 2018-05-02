using System;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServer.Responses;
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
        
            return new Response(new TestStatusCode(), request);
        }
    }

    internal class TestRequest : Request
    {
        public TestRequest() : base(RequestType.GET, "/", string.Empty, HttpVersion.Version11)
        {
        }
    }

    internal class TestStatusCode : IHttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "yes";
    }
}