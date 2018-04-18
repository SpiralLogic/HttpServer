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
        [Fact]
        public void HttpServerCanStart()
        {
            var logger = new TestLogger();
            using (var server = new Server(logger))
            {
                server.Start();
                Assert.True(server.IsRunning);
            }
        }

        [Fact]
        public void HttpServerCanStop()
        {
            var logger = new TestLogger();
            using (var server = new Server(logger))
            {
                server.Start();
                server.Stop();
                Assert.False(server.IsRunning);
            }
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8111;
            var logger = new TestLogger();
            using (var server = new Server(logger, testPort))
            {
                server.Start();

                Assert.Equal(testPort, server.Port);
            }
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestString = "TEST";

            var logger = new TestLogger();
            using (var server = new Server(logger))
            {
                server.Start();

                WriteToListener(server.Port, server.Encoding, requestString);
                Assert.Equal(requestString, logger.LastMessage);
            }
        }

        private static void WriteToListener(int port, Encoding encoding, string messageString)
        {
            using (var client = new TcpClient())
            {
                client.Connect(IPAddress.Loopback, port);
                var stream = client.GetStream();
                var writeBuffer = encoding.GetBytes(messageString);
                stream.Write(writeBuffer);
            }
        }
    }

    internal class TestLogger : ILogger
    {
        internal string LastMessage { get; private set; } = string.Empty;

        public void Log(string message)
        {
            LastMessage = message;
        }
    }
}