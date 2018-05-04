using System;
using System.IO;
using System.Text;
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
            var request = new Request(RequestTypes.Get, "/", "", _httpVersion);
            var directoryHandler = new DirectoryHandler(TestDirectory);

            var response = directoryHandler.Handle(request);

            Assert.True(response.StringBody.StartsWith("<html>"));
        }

        [Fact]
        public void BodyHasFileContentsWhenFileIsProvided()
        {
            var request = new Request(RequestTypes.Get, "/test.txt", "test.txt", _httpVersion);
            var directoryHandler = new DirectoryHandler(TestDirectory);

            var response = directoryHandler.Handle(request);

            Assert.Equal(Encoding.UTF8.GetBytes("Test File"), response.BodyBytes);
        }
    }
}