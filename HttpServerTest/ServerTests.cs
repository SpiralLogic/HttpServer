using System;
using System.Text;
using HttpServer;
using HttpServer.Listeners;
using HttpServer.Loggers;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        private readonly TestListener _listener;
        private readonly Server _server;
        private readonly TestLogger _logger;

        public ServerTests()
        {
            _logger = new TestLogger();
            _listener = new TestListener();
            _server = new Server(_listener, _logger);
        }

        [Fact]
        public void HttpServerCanStart()
        {
            _server.Start();
            Assert.True(_server.IsRunning);
            Assert.True(_listener.IsListening);
        }

        [Fact]
        public void HttpServerCanStop()
        {
            _server.Start();
            _server.Stop();
            Assert.False(_server.IsRunning);
            Assert.False(_listener.IsListening);
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
    }

    internal class TestLogger : ILogger
    {
        internal string Message { get; private set; } = string.Empty;

        public void Log(string message)
        {
            Message = message;
        }
    }
}