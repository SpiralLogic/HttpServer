using HttpServer.RequestHandlers;
using Xunit;

namespace HttpServerTest
{
    public class HttpRequestHandlerTests
    {
        private readonly HttpRequestHandler _handler;

        public HttpRequestHandlerTests()
        {
            _handler = new HttpRequestHandler();
        }

        [Fact]
        public void RequestsReturnResponse()
        {
            const string requestString = "GET / HTTP/1.1\n";
            Assert.IsAssignableFrom<HttpRequest>(_handler.ParseRequest(requestString));
        }

        [Fact]
        public void UnknownRequestReturnsResponseWithTypeUnknown()
        {
            const string requestString = "What / HTTP/1.1\n";
            var request = _handler.ParseRequest(requestString);

            Assert.Equal(RequestType.UNKNOWN, request.Type);
        }
    }
}