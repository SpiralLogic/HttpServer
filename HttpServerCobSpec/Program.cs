using HttpServer;

namespace HttpServerCobSpec
{
    class Program
    {
        static void Main()
        {
            var logger = new ConsoleLogger();
            var server = new Server(new Router(), logger);
            server.Start(5000);
            while (server.IsRunning)
            {
            }
        }
    }
}