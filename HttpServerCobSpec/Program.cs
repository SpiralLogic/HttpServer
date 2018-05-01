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
            
            router.AddDirectoryRoute(RequestType.GET, "/", new DirectoryHandler());
            router.AddRoute(RequestType.POST, "/form", new FormHandler());
            router.AddRoute(RequestType.PUT, "/put-target", new FormHandler());

            router.AddRoute(RequestType.GET, "/method_options", new FormHandler());
            router.AddRoute(RequestType.PUT, "/method_options", new FormHandler());
            router.AddRoute(RequestType.POST, "/method_options", new FormHandler());

            router.AddRoute(RequestType.GET, "/method_options2", new FormHandler());
            router.AddRoute(RequestType.GET, "/coffee", new Handler418());
            router.AddRoute(RequestType.GET, "/tea", new FormHandler());

            router.AddRoute(RequestType.GET, "/redirect", new RedirectHandler("/"));
            
            var crudTestHandler = new CrudHandler("/cat-form/data");

            router.AddRoute(RequestType.GET, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestType.POST, "/cat-form", crudTestHandler);
            router.AddRoute(RequestType.PUT, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestType.DELETE, "/cat-form/data", crudTestHandler);
            
            router.AddRoute(RequestType.PATCH, "/patch-content.txt", new PatchHandler());
            
            return router;
        }
    }
}