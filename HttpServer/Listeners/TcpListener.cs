using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpServer.Listeners;

namespace HttpServer
{
    public class TcpListener : IListener
    {
        private const int BufferSize = 1024;

        private System.Net.Sockets.TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly IRequestHandler _requestHandler;

        public Encoding Encoding { get; } = Encoding.UTF8;
        public bool IsListening { get; private set; }
        public int Port => !IsListening ? _port : ((IPEndPoint) _listener.Server.LocalEndPoint).Port;

        public TcpListener(IRequestHandler requestHandler, IPAddress ipAddress = null, int port = 0)
        {
            if (port < 0) throw new ArgumentException(nameof(port));
            _ipAddress = ipAddress ?? IPAddress.Loopback;
            _requestHandler = requestHandler;
            _port = port;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _listener = new System.Net.Sockets.TcpListener(_ipAddress, _port);
                _listener.Start();
                Task.Run(() => { Listen(); }, _cancellationTokenSource.Token);
                IsListening = true;
            }
            catch (TaskCanceledException)
            {
                IsListening = false;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _listener.Stop();
            _listener = null;
            IsListening = false;
        }

        private void Listen()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!_listener.Pending()) continue;

                ProcessRequest();
            }
        }

        private void ProcessRequest()
        {
            var client = _listener.AcceptTcpClient();
            var bytes = new byte[BufferSize];
            var clientStream = client.GetStream();

            if (clientStream.CanRead)
            {
                var i = clientStream.Read(bytes, 0, BufferSize);
                var data = Encoding.GetString(bytes, 0, i);

                var response = _requestHandler.HandleRequest(data);
                var result = Encoding.GetBytes(response);

                clientStream.Write(result, 0, result.Length);
            }

            clientStream.Close();
            client.Close();
        }
    }
}