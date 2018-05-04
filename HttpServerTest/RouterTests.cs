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
        private const string Resource = "/test";
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
            _uri = CreateRequestUri(Resource);
            _testHandler = new TestRequestHandler();
            _testClient = new HttpClient();
        }

        [Fact]
        public async Task UnsupportedRequestReturnsMethodNotAllowed()
        {
            _router.AddRoute(RequestTypes.Get, Resource, _testHandler);

            var request = new HttpRequestMessage(new HttpMethod("wawa"), CreateRequestUri("wawa"));
            var response = await _testClient.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    
        [Fact]
        public async Task CanCreateGetRoute()
        {
            _router.AddRoute(RequestTypes.Get, Resource, _testHandler);

            await _testClient.GetStringAsync(_uri);

            Assert.Equal(Resource, _testHandler.LastRequest.Resource);
            Assert.Equal(RequestTypes.Get, _testHandler.LastRequest.Type);
        }

        [Fact]
        public async Task CanCreatePostRoute()
        {
            _router.AddRoute(RequestTypes.Post, Resource, _testHandler);

            var request = new HttpRequestMessage(HttpMethod.Post, _uri);
            var response = await _testClient.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(RequestTypes.Post, _testHandler.LastRequest.Type);
            Assert.Equal(Resource, _testHandler.LastRequest.Resource);
        }

        [Fact]
        public async Task HeadRequestHasNoBody()
        {
            _router.AddRoute(RequestTypes.Get, Resource, _testHandler);

            var request = new HttpRequestMessage(HttpMethod.Head, _uri);
            var response = await _testClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);
            Assert.Empty(responseBody);
        }

        [Fact]
        public async Task GetRequestToFileHasFileContentsInBody()
        {
            _router.AddRoute(RequestTypes.Get, Resource, _testHandler);

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