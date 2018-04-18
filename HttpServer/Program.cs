using System.Net;
using HttpServer.Loggers;
using HttpServer.RequestHandlers;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var listener = new TcpListener(new HttpRequestHandler(), IPAddress.Loopback, 8080);
            var server = new Server(listener, logger);

            server.Start();
            logger.Log("Waiting for connection on port: " + listener.Port);
            while (server.IsRunning)
            {
            }
        }
    }
}