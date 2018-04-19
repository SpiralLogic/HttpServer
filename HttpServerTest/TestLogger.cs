using System;
using HttpServer.Loggers;

namespace HttpServerTest
{
    internal class TestLogger : ILogger
    {
        internal string LastMessage { get; private set; } = string.Empty;
        internal event EventHandler LogWrittenEvent;

        public void Log(string message)
        {
            LastMessage = message;
            LogWrittenEvent?.Invoke(this, new EventArgs());
        }
    }
}