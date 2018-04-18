using System;
using System.Text;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        [Fact]
        public void HttpServerCanStart()
        {
            var listener = new TestListener();
            var server = new Server(listener);
            server.Start();
            Assert.True(server.IsRunning);
            Assert.True(listener.IsListening);
        }
        
        [Fact]
        public void HttpServerCanStop()
        {
            var listener = new TestListener();
            var server = new Server(listener);
            server.Start();
            server.Stop();
            Assert.False(server.IsRunning);
            Assert.False(listener.IsListening);
        }
    }


    internal class TestListener : IListener
    {
        public void Start()
        {
            IsListening = true;
        }

        public void Stop()
        {
            IsListening = false;
        }

        public bool IsListening { get; private set; }
        public Encoding Encoding { get; } = Encoding.ASCII;
        public int Port { get; } = 8000;
        public event EventHandler<RequestReceivedEventArgs> RequestReceived;
    }
}