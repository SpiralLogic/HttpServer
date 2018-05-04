﻿using System;
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

            router.AddDirectoryRoute(RequestType.GET, "/", new DirectoryHandler());
       //     router.AddRoute(RequestType.GET, "/partial_content.txt", new PartialContentTest());
            router.AddRoute(RequestType.POST, "/form", new FormHandler());
            router.AddRoute(RequestType.PUT, "/put-target", new FormHandler());

            router.AddRoute(RequestType.GET, "/method_options", new StringContentHandler());
            router.AddRoute(RequestType.PUT, "/method_options", new StringContentHandler());
            router.AddRoute(RequestType.POST, "/method_options", new StringContentHandler());
            router.AddRoute(RequestType.GET, "/method_options2", new StringContentHandler());

            router.AddRoute(RequestType.GET, "/coffee", new Handler418());
            router.AddRoute(RequestType.GET, "/tea", new StringContentHandler());
            router.AddRoute(RequestType.GET, "/parameters", new ParameterPrintHandler());

            router.AddRoute(RequestType.GET, "/redirect", new RedirectHandler("/"));

            router.AddRoute(RequestType.PATCH, "/patch-content.txt", new PatchHandler());
            router.AddRoute(RequestType.GET, "/cookie", new CookieHandler("Eat"));
            router.AddRoute(RequestType.GET, "/eat_cookie", new CookieHandler("mmmm chocolate"));

            router.AddRoute(RequestType.GET, "/logs", new BasicAuthHandler("admin", "hunter2"));
            var crudTestHandler = new CrudHandler("/cat-form/data");

            router.AddRoute(RequestType.GET, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestType.POST, "/cat-form", crudTestHandler);
            router.AddRoute(RequestType.PUT, "/cat-form/data", crudTestHandler);
            router.AddRoute(RequestType.DELETE, "/cat-form/data", crudTestHandler);

            return router;
        }
    }
}