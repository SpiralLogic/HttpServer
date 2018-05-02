namespace HttpServer.Listeners
{
    public interface IListener
    {
        void Start();
        void Stop();
        bool IsListening { get; }
        int Port { get; set; }
    }
}