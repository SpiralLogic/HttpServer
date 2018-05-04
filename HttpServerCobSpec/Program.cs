using System;
using HttpServer;
using HttpServer.Handlers;
using HttpServer.RequestHandlers;
using HttpServerCobSpec.Handlers;

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
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            while (server.IsRunning)
            {
            }
        }

        private static Router CreateRouter()
        {
            var router = new Router();

            router.AddDirectoryRoute(RequestTypes.Get, "/", new DirectoryHandler());
            router.AddRoute(RequestTypes.Post, "/form", new StringContentHandler());
            router.AddRoute(RequestTypes.Put, "/put-target", new StringContentHandler());

            router.AddRoute(RequestTypes.Get, "/method_options", new StringContentHandler());
            router.AddRoute(RequestTypes.Put, "/method_options", new StringContentHandler());
            router.AddRoute(RequestTypes.Post, "/method_options", new StringContentHandler());
            router.AddRoute(RequestTypes.Get, "/method_options2", new StringContentHandler());

            router.AddRoute(RequestTypes.Get, "/coffee", new Handler418());
            router.AddRoute(RequestTypes.Get, "/tea", new StringContentHandler());
            router.AddRoute(RequestTypes.Get, "/parameters", new ParameterPrintHandler());

            router.AddRoute(RequestTypes.Get, "/redirect", new RedirectHandler("/"));

            router.AddRoute(RequestTypes.Patch, "/patch-content.txt", new PatchHandler());
            router.AddRoute(RequestTypes.Get, "/cookie", new CookieHandler("Eat"));
            router.AddRoute(RequestTypes.Get, "/eat_cookie", new CookieHandler("mmmm chocolate"));

            router.AddRoute(RequestTypes.Get, "/logs", new BasicAuthHandler("admin", "hunter2"));
 
            var crudTestHandler = new CrudHandler("/cat-form/data");

            router.AddRoute(RequestTypes.Get, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestTypes.Post, "/cat-form", crudTestHandler);
            router.AddRoute(RequestTypes.Put, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestTypes.Delete, "/cat-form/data", crudTestHandler);

            return router;
        }
    }
}