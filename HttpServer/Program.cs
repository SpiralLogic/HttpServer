using System.Net;
using HttpServer.Listeners;
using HttpServer.Loggers;

namespace HttpServer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var server = new Server(logger, 8080);

            server.Start();
            while (server.IsRunning)
            {
            }
        }
    }
}