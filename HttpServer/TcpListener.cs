using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class TcpListener : IListener
    {
        private const int BufferSize = 1024;
        private System.Net.Sockets.TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        
        public Encoding Encoding { get; } = Encoding.Unicode;
        public bool IsListening { get; private set; }
        public int Port => !IsListening ? _port : ((IPEndPoint) _listener.Server.LocalEndPoint).Port;

        public event EventHandler<RequestReceivedEventArgs> RequestReceived;

        public TcpListener(IPAddress ipAddress = null, int port = 0)
        {
            if (port < 0) throw new ArgumentException(nameof(port));
            _ipAddress = ipAddress ?? IPAddress.Loopback;
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
            var bytes = new byte[BufferSize];
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!_listener.Pending()) continue;

                ProcessRequest(bytes);
            }
        }

        private void ProcessRequest(byte[] bytes)
        {
            var data = string.Empty;
            var client = _listener.AcceptTcpClient();
            var clientStream = client.GetStream();
            int i;

            while ((i = clientStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data += Encoding.GetString(bytes, 0, i);
            }

            RequestReceived?.Invoke(this, new RequestReceivedEventArgs(data));
            client.Close();
        }
    }
}