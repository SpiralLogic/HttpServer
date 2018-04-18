using System;
using System.Net;
using System.Net.Sockets;

namespace HttpServer
{
    public class Server 
    {
        private readonly IListener _listener;
        public bool IsRunning => _listener.IsListening;

        public Server(IListener listener)
        {
            _listener = listener;
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        ~Server()
        {
            _listener.Stop();
        }
    }
}