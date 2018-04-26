using System;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        private readonly Server _server;

        public ServerTests()
        {
            var logger = new TestLogger();
            _server = new Server(new Router(), logger);
        }

        [Fact]
        public void HttpServerCanStart()
        {
            _server.Start();
            Assert.True(_server.IsRunning);
        }

        [Fact]
        public void HttpServerCantStartTwice()
        {
            _server.Start();
            Assert.Throws<ApplicationException>(() => _server.Start());
        }

        [Fact]
        public void HttpServerCanStop()
        {
            _server.Start();
            _server.Stop();
            Assert.False(_server.IsRunning);
        }

        [Fact]
        public void PortCantBeTooLarge()
        {
            const int testPort = 65536;
            Assert.Throws<ArgumentException>(() => _server.Start(testPort));
        }

        [Fact]
        public void PortCantBeLessThanZero()
        {
            const int testPort = -1;
            Assert.Throws<ArgumentException>(() => _server.Start(testPort));
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8765;
            _server.Start(testPort);
            Assert.Equal(testPort, _server.Port);
        }

        [Fact]
        public void WhenServerIsRunningCantChangePort()
        {
            const int testPort = 8765;
            _server.Start();
            Assert.Throws<ApplicationException>(() => _server.Start(testPort));
        }

        ~ServerTests()
        {
            _server.Dispose();
        }
    }
}