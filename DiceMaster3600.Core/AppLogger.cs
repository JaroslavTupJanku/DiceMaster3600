using Serilog;
using System;

namespace DiceMaster3600.Core
{
    public static class AppLogger
    {
        private static ILogger? logger;

        public static void ConfigureLogger()
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Information(string message) => logger?.Information(message);
        public static void Warning(string message) => logger?.Warning(message);
        public static void Error(string message) => logger?.Error(message);
        public static void Error(Exception ex, string message) => logger?.Error(ex, message);
        public static void Debug(string message) => logger?.Debug(message);
        public static void Fatal(string message) => logger?.Fatal(message);
    }
}
