using System;
using System.Text;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
    public class ServerTests
    {
        private readonly TestListener _listener;
        private readonly TestLogger _logger;
        private readonly Server _server;

        public ServerTests()
        {
            _listener = new TestListener();
            _logger = new TestLogger();
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

        [Fact]
        public void HttpServerCanWriteRequestsToLogger()
        {
            const string requestString = "Test Request";
            
            _server.Start();
            _listener.SimulateRequest(requestString);
            
            Assert.Equal(requestString, _logger.Message);
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

        public void SimulateRequest(string request)
        {
            RequestReceived?.Invoke(this, new RequestReceivedEventArgs(request));
        }

        public bool IsListening { get; private set; }
        public Encoding Encoding { get; } = Encoding.ASCII;
        public int Port { get; } = 8000;
        public event EventHandler<RequestReceivedEventArgs> RequestReceived;
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