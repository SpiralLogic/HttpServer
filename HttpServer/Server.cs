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
        private const uint LargestPort = 65535;
        
        private readonly ILogger _logger;
        private readonly IListener _listener;
        private readonly HttpRequestHandler _handler;
        
        public int Port => _listener.Port;
        public Encoding Encoding => _listener.Encoding;
        public bool IsRunning => _listener.IsListening;

        public Server(ILogger logger, uint port = 0)
        {
            if (port > LargestPort) throw new ArgumentException(nameof(port));
            
            _logger = logger;
            _handler = new HttpRequestHandler(new HttpRequestParser(), logger);
            _listener = new TcpListener(_handler, IPAddress.Loopback, (int) port);
        }

        public void Start()
        {
            if (_listener.IsListening) throw new ApplicationException("Server is already running");
            
            _listener.Start();
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