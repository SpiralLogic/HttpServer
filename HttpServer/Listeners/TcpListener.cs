using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer.Listeners
{
    internal class TcpListener : IListener
    {
        private const int BufferSize = 1024;
        private const uint LargestPort = 65535;

        private readonly IPAddress _ipAddress;

        private int _port;
        private System.Net.Sockets.TcpListener _tcpSocketListener;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Func<string, byte[]> _handleFunction;

        public bool IsListening { get; private set; }

        public TcpListener(Func<string, byte[]> handleFunction, IPAddress ipAddress = null, int port = 0)
        {
            _ipAddress = ipAddress ?? IPAddress.Loopback;
            _handleFunction = handleFunction;
            _port = port;
        }

        public int Port
        {
            get => !IsListening ? _port : ((IPEndPoint) _tcpSocketListener.Server.LocalEndPoint).Port;
            set
            {
                if (IsListening) throw new ApplicationException("Server is already listening");
                if (value < 0 || value > LargestPort) throw new ArgumentException("Port must be less that 65535 and 0 or greater");
                _port = value;
            }
        }

        public void Start()
        {
            var listenerStartedEvent = new ManualResetEventSlim(false);
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _tcpSocketListener = new System.Net.Sockets.TcpListener(_ipAddress, _port);
                _tcpSocketListener.Start();
                Task.Run(() => { Listen(listenerStartedEvent); }, _cancellationTokenSource.Token);
                listenerStartedEvent.Wait(_cancellationTokenSource.Token);
                IsListening = true;
            }
            catch (TaskCanceledException)
            {
                IsListening = false;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel(true);
            _tcpSocketListener.Stop();
            _tcpSocketListener = null;
            IsListening = false;
        }

        private void Listen(ManualResetEventSlim listenerStartedEvent)
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                listenerStartedEvent.Set();
                if (_tcpSocketListener.Pending())
                {
                    Task.Run(() => ProcessRequest(), _cancellationTokenSource.Token);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void ProcessRequest()
        {
            var client = _tcpSocketListener.AcceptTcpClient();
            var clientStream = client.GetStream();

            if (clientStream.CanRead)
            {
                var data = ReadClientStream(clientStream);

                RespondToRequest(data, clientStream);
            }

            clientStream.Close();
            client.Close();
        }

        private string ReadClientStream(Stream clientStream)
        {
            var bytes = new byte[BufferSize];
            var bytesRead = clientStream.Read(bytes, 0, BufferSize);
            var data = Encoding.UTF8.GetString(bytes, 0, bytesRead);

            return data;
        }

        private void RespondToRequest(string data, Stream clientStream)
        {
            var response = _handleFunction(data);

            clientStream.Write(response, 0, response.Length);
        }
    }
}