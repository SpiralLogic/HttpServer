using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerRequestTests
    {
        private readonly Server _server;
        private readonly TestRequestHandler _handler;
        private readonly ManualResetEventSlim _requestReceivedEvent;
        
        public ServerRequestTests()
        {
            _requestReceivedEvent = new ManualResetEventSlim(false);
            
            _handler = new TestRequestHandler();
            _handler.RequestRecievedEvent += (o,a) => _requestReceivedEvent.Set();
            
            var logger = new TestLogger();
            _server = new Server(_handler, logger);
            
            _server.Start();
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestString = "TEST";

            SimulateRequest(_server.Port, _server.Encoding, requestString);
            _requestReceivedEvent.Wait(1000);

            Assert.Equal(requestString, _handler.RequestString);
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