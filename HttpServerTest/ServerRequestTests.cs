using System.Net;
using System.Net.Sockets;
using System.Text;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerRequestTests
    {
        private readonly Server _server;
        private readonly TestLogger _logger;
        private TestRequestHandler _handler;
        
        public ServerRequestTests()
        {
            _handler = new TestRequestHandler();
            _logger = new TestLogger();
            _server = new Server(_handler, _logger);
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestString = "TEST";
            _server.Start();

            _logger.LogWrittenEvent += (o, a) => Assert.Equal(requestString, _logger.LastMessage);
            SimulateRequest(_server.Port, _server.Encoding, requestString);
        }

        [Fact]
        public void CanRecieveGetRequests()
        {
            const string requestString = "GET / HTTP/1.1\n";
            _server.Start();

            _logger.LogWrittenEvent += (o, a) => Assert.Equal(requestString, _logger.LastMessage);
            SimulateRequest(_server.Port, _server.Encoding, requestString);
        }

        private string SimulateRequest(int port, Encoding encoding, string messageString)
        {
            var writeBuffer = encoding.GetBytes(messageString);
            var readBuffer = new byte[1024];
            
            using (var client = new TcpClient())
            {
                client.Connect(IPAddress.Loopback, port);
                var stream = client.GetStream();
                stream.Write(writeBuffer);

                stream.Read(readBuffer, 0, readBuffer.Length);
            }

            return encoding.GetString(readBuffer);
        }

        ~ServerRequestTests()
        {
            _server.Dispose();
        }
    }
}