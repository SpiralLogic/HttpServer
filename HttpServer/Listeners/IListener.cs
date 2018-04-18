using System.Text;
using HttpServer.RequestHandlers;

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