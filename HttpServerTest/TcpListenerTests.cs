using System.Net;
using System.Net.Sockets;
using System.Text;
using Xunit;
using TcpListener = HttpServer.TcpListener;

namespace HttpServerTest
{
    public class TcpListenerTests
    {
        [Fact]
        public void CanStart()
        {
            var listener = new TcpListener(IPAddress.Loopback);
            listener.Start();
            Assert.True(listener.IsListening);
        }

        [Fact]
        public void CanStop()
        {
            var listener = new TcpListener(IPAddress.Loopback);
            listener.Start();
            listener.Stop();

            Assert.False(listener.IsListening);
        }

        [Fact]
        public void CanListenOnSpecifiedPort()
        {
            const int testPort = 8111;
            var listener = new TcpListener(IPAddress.Loopback, testPort);
            listener.Start();

            Assert.Equal(testPort, listener.Port);
        }

        [Fact]
        public void CanReceiveRequests()
        {
            const string requestMessage = "TEST";
            
            var listener = new TcpListener(IPAddress.Loopback);
            listener.Start();
            var request = string.Empty;
            listener.RequestReceived += (sender, args) => { request = args.RequestString; };

            WriteToListner(listener.Port, listener.Encoding, requestMessage);

            Assert.Equal(requestMessage, request);
        }

        private static void WriteToListner(int port, Encoding encoding, string messageString)
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
}