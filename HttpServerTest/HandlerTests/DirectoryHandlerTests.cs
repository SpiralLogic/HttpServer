using System;
using System.IO;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using Xunit;

namespace HttpServerTest.HandlerTests
{
    public class DirectoryHandlerTests
    {
        private static string TestDirectory => Path.Combine(Directory.GetCurrentDirectory(), "../../../", "wwwroot");
        private readonly Version _httpVersion = HttpVersion.Version11;

        [Fact]
        public void BodyHasDirectoryLinksWhenNoFileProvided()
        {
            var request = new Request(RequestType.GET, "/", "", _httpVersion);
            var directoryHandler = new DirectoryHandler(TestDirectory);

            var response = directoryHandler.Handle(request);

            Assert.True(response.Body.StartsWith("<html>"));
        }

        [Fact]
        public void BodyHasFileContentsWhenFileIsProvided()
        {
            var request = new Request(RequestType.GET, "/test.txt", "test.txt", _httpVersion);
            var directoryHandler = new DirectoryHandler(TestDirectory);

            var response = directoryHandler.Handle(request);

            Assert.Equal("Test File", response.Body);
        }
    }
}