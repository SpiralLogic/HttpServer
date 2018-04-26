using System;
using HttpServer;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;

namespace HttpServerCobSpec
{
    class Program
    {
        static void Main()
        {
            var logger = new ConsoleLogger();
            var server = new Server(CreateRouter(), logger);
            try
            {
                server.Start(5000);
            }
            catch (Exception)
            {
            }

            while (server.IsRunning)
            {
            }
        }

        private static Router CreateRouter()
        {
            var router = new Router();
            
            router.AddRoute(RequestType.GET, "/", new DirectoryHandler());
            router.AddRoute(RequestType.POST, "/form", new FormHandler());
            router.AddRoute(RequestType.PUT, "/put-target", new FormHandler());

            return router;
        }
    }
}