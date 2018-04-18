using System.Text;

namespace HttpServer.Listeners
{
    public interface IListener
    {
        void Start();
        void Stop();
        bool IsListening { get; }
        Encoding Encoding { get; }
        int Port { get; }
    }
}