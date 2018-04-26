using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpServer;
using HttpServer.RequestHandlers;
using Xunit;

namespace HttpServerTest
{
    public class RouterTests
    {
        private const string Path = "/test";
        private readonly Server _server;
        private readonly Uri _uri;
        private readonly Router _router;
        private readonly HttpClient _testClient;
        private readonly TestRequestHandler _testHandler;

        public RouterTests()
        {
            _router = new Router();
            _server = new Server(_router, new TestLogger());
            _server.Start();
            _uri = CreateRequestUri(Path);
            _testHandler = new TestRequestHandler();
            _testClient = new HttpClient();
        }

        [Fact]
        public async Task UnsupportedRequestReturnsBadRequest()
        {
            _router.AddRoute(RequestType.GET, Path, _testHandler);

            var request = new HttpRequestMessage(new HttpMethod("wawa"), _uri);
            var response = await _testClient.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CanCreateGetRoute()
        {
            _router.AddRoute(RequestType.GET, Path, _testHandler);

            await _testClient.GetStringAsync(_uri);

            Assert.Equal(RequestType.GET, _testHandler.LastRequest.Type);
            Assert.Equal(Path, _testHandler.LastRequest.Resource);
        }
/*

        [Fact]
        public async Task CanCreatePostRoute()
        {
            _router.AddRoute(RequestType.POST, Path, _testHandler);

            await _testClient.GetStringAsync(_uri);

            Assert.Equal(RequestType.POST, _testHandler.LastRequest.Type);
            Assert.Equal(Path, _testHandler.LastRequest.Resource);
        }
*/

        [Fact]
        public async Task HeadRequestHasNoBody()
        {
            _router.AddRoute(RequestType.GET, Path, _testHandler);

            var request = new HttpRequestMessage(HttpMethod.Head, _uri);
            var response = await _testClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);
            Assert.Empty(responseBody);
        }

        private Uri CreateRequestUri(string path)
        {
            var requestUri = new UriBuilder
            {
                Host = "localhost",
                Port = _server.Port,
                Path = path
            };
            Assert.NotNull(requestUri.Uri);
            return requestUri.Uri;
        }

        ~RouterTests()
        {
            _server.Dispose();
            _testClient.CancelPendingRequests();
            _testClient.Dispose();
        }
    }
}