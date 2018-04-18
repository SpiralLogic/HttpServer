using System.Net;
using System.Net.Sockets;
using System.Text;
using HttpServer.RequestHandlers;
using Xunit;
using TcpListener = HttpServer.Listeners.TcpListener;

namespace HttpServerTest
{
    public class TcpListenerTests
    {
        [Fact]
        public void CanStart()
        {
            var requestHandler = new TestRequestHandler();
            var listener = new TcpListener(requestHandler, IPAddress.Loopback);
            listener.Start();
            Assert.True(listener.IsListening);
        }

        [Fact]
        public void CanStop()
        {
            var requestHandler = new TestRequestHandler();
            var listener = new TcpListener(requestHandler, IPAddress.Loopback);
            listener.Start();
            listener.Stop();

            Assert.False(listener.IsListening);
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8111;
            var requestHandler = new TestRequestHandler();
            var listener = new TcpListener(requestHandler, IPAddress.Loopback, testPort);
            listener.Start();

            Assert.Equal(testPort, listener.Port);
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestString = "TEST";
            var requestHandler = new TestRequestHandler();
            var listener = new TcpListener(requestHandler, IPAddress.Loopback);
            var request = string.Empty;

            listener.Start();

            WriteToListener(listener.Port, listener.Encoding, requestString);

            Assert.Equal(requestString, requestHandler.LastRequest);
        }

        private static void WriteToListener(int port, Encoding encoding, string messageString)
        {
            using (var client = new TcpClient())
            {
                client.Connect("localhost", port);
                var stream = client.GetStream();
                var writeBuffer = encoding.GetBytes(messageString);
                stream.Write(writeBuffer);
            }
        }
    }

    internal class TestRequestHandler : IRequestHandler
    {
        public string LastRequest { get; private set; }
        
        public string HandleRequest(string request)
        {
            LastRequest = request;
            return request;
        }
    }
}