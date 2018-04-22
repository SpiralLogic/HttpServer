using System;
using HttpServer.RequestHandlers;

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

        public string CreateResponse(HttpRequest request)
        {
            return "Good";            
        }
    }

    internal class TestHttpRequest : HttpRequest
    {
        public TestHttpRequest() : base(RequestType.GET, "/", HttpVersion.Version11)
        {
        }
    }
}