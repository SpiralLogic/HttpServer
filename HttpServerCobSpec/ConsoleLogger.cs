using System;
using HttpServer.Loggers;

namespace HttpServerCobSpec
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}