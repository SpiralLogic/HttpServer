using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HttpServer;
using HttpServer.Loggers;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        private readonly Server _server;
        private readonly TestLogger _logger;

        public ServerTests()
        {
            _logger = new TestLogger();
            _server = new Server(_logger);
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
            Assert.Throws<ArgumentException>(() => new Server(new TestLogger(), testPort));
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8765;
            using (var server = new Server(new TestLogger(), testPort))
            {
                server.Start();
                Assert.Equal(testPort, server.Port);
            }
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestString = "TEST";
            _server.Start();

            _logger.LogWrittenEvent += (o, a) => Assert.Equal(requestString, _logger.LastMessage);
            SimulateRequest(_server.Port, _server.Encoding, requestString);
        }

        private void SimulateRequest(int port, Encoding encoding, string messageString)
        {
            using (var client = new TcpClient())
            {
                client.Connect(IPAddress.Loopback, port);
                var stream = client.GetStream();
                var writeBuffer = encoding.GetBytes(messageString);
                stream.Write(writeBuffer);
            }
        }

        ~ServerTests()
        {
            _server.Dispose();
        }
    }

    internal class TestLogger : ILogger
    {
        internal string LastMessage { get; private set; } = string.Empty;
        internal event EventHandler LogWrittenEvent;

        public void Log(string message)
        {
            LastMessage = message;
            LogWrittenEvent?.Invoke(this, new EventArgs());
        }
    }
}