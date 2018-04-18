using HttpServer.Listeners;
using HttpServer.Loggers;

namespace HttpServer
{
    public class Server
    {
        private readonly IListener _listener;
        public bool IsRunning => _listener.IsListening;

        public Server(IListener listener, ILogger logger)
        {
            _listener = listener;
        }

        public void Start()
        {
            if (!_listener.IsListening) _listener.Start();
        }

        public void Stop()
        {
            if (_listener.IsListening )_listener.Stop();
        }

        ~Server()
        {
            _listener.Stop();
        }
    }
}