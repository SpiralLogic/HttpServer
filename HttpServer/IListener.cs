using System;
using System.Text;

namespace HttpServer
{
    public interface IListener
    {
        void Start();
        void Stop();
        bool IsListening { get; }
        Encoding Encoding { get; }
        int Port { get; }
        event EventHandler<RequestReceivedEventArgs> RequestReceived;
    }

    public class RequestReceivedEventArgs
    {
        public RequestReceivedEventArgs(string requestString)
        {
            RequestString = requestString;
        }

        public string RequestString { get; }
    }
}