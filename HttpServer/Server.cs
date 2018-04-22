using System;
using System.IO;
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

        public Encoding Encoding => _listener.Encoding;
        public bool IsRunning => _listener.IsListening;

        public Server(IRequestHandler handler, ILogger logger)
        {
            _logger = logger;
            _listener = new TcpListener(handler, IPAddress.Loopback);
            }

        public int Port
        {
            get => _listener.Port;
            set => _listener.Port = value;
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