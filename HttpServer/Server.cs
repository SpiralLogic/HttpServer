namespace HttpServer
{
    public class Server 
    {
        private readonly IListener _listener;
        private ILogger _logger;
        public bool IsRunning => _listener.IsListening;

        public Server(IListener listener, ILogger logger)
        {
            _logger = logger;
            _listener = listener;
            _listener.RequestReceived += (sender, args) => ProcessRequest(args.RequestString);
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void ProcessRequest(string requestString)
        {
            _logger.Log(requestString);
        }
        
        ~Server()
        {
            _listener.Stop();
        }
    }
}