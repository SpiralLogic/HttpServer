using System;
using HttpServer.RequestHandlers;
using HttpServer.RequestHandlers.ResponseCodes;

namespace HttpServerTest
{
    internal class TestRequestHandler : IRequestHandler
    {
        internal event EventHandler RequestRecievedEvent;
        internal string RequestString { get; private set; }

        public HttpRequest ParseRequest(string request)
        {
            RequestString = request;
            RequestRecievedEvent?.Invoke(this, new EventArgs());

            return new TestHttpRequest();
        }

        public Response CreateResponse(HttpRequest request)
        {
            return new Response(new TestStatusCode());
        }
    }

    internal class TestHttpRequest : HttpRequest
    {
        public TestHttpRequest() : base(RequestType.GET, "/", HttpVersion.Version11)
        {
        }
    }

    internal class TestStatusCode : IHttpStatusCode
    {
        public int Code { get; } = 200;
        public string Status { get; } = "yes";
    }
}