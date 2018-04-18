using System;
using System.Net;
using System.Text;
using HttpServer.Listeners;
using HttpServer.Loggers;
using HttpServer.RequestHandlers;

namespace HttpServer
{
    public class Server : IDisposable
    {
        private readonly ILogger _logger;
        private readonly IListener _listener;
        private readonly HttpRequestHandler _handler;
        public int Port => _listener.Port;
        public Encoding Encoding => _listener.Encoding;
        public bool IsRunning => _listener.IsListening;

        public Server(ILogger logger, int port = 0)
        {
            _logger = logger;
            _handler = new HttpRequestHandler(new HttpRequestParser(), logger);
            _listener = new TcpListener(_handler, IPAddress.Loopback, port);
        }

        public void Start()
        {
            if (!_listener.IsListening) _listener.Start();
            _logger.Log("Waiting for connection on port: " + _listener.Port);
        }

        public void Stop()
        {
            if (_listener.IsListening) _listener.Stop();
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}