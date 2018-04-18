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
            var server = new Server(logger);

            server.Start();
            while (server.IsRunning)
            {
            }
        }
    }
}