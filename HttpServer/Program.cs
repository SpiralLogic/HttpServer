using HttpServer.Loggers;
using HttpServer.RequestHandlers;

namespace HttpServer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var handler = new HttpRequestHandler();
            var server = new Server(handler, logger);
            server.Port = 5000;
            server.Start();
            while (server.IsRunning)
            {
            }
        }
    }
}