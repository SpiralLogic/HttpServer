using System;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        [Fact]
        public void HttpServerCanStart()
        {
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                server.Start();
                Assert.True(server.IsRunning);
            }
        }

        [Fact]
        public void HttpServerCantStartTwice()
        {
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                server.Start();
                Assert.Throws<ApplicationException>(() => server.Start());
            }
        }

        [Fact]
        public void HttpServerCanStop()
        {
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                server.Start();
                server.Stop();
                Assert.False(server.IsRunning);
            }
        }

        [Fact]
        public void PortCantBeTooLarge()
        {
            const int testPort = 65536;
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                Assert.Throws<ArgumentException>(() => server.Port = testPort);
            }
        }

        [Fact]
        public void PortCantBeLessThanZero()
        {
            const int testPort = -1;
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                Assert.Throws<ArgumentException>(() => server.Port = testPort);
            }
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8765;
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                server.Port = testPort;
                server.Start();
                Assert.Equal(testPort, server.Port);
            }
        }
        
        [Fact]
        public void WhenServerIsRunningCantChangePort()
        {
            const int testPort = 8765;
            var handler = new TestRequestHandler();
            var logger = new TestLogger();
            using (var server = new Server(handler, logger))
            {
                server.Start();
                Assert.Throws<ArgumentException>(() => server.Port = testPort);
            }
        }
    }
}