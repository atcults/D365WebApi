using System;
using Microsoft.Extensions.Logging;

namespace D365WebApi.LogExtensions
{
    public static class LoggerExtensionsForApplication
    {
        private static readonly Action<ILogger, Exception> ApplicationStartedAction;

        private static readonly Action<ILogger, Exception> ApplicationExitedAction;

        private static readonly Action<ILogger, Exception> ApplicationFaultedAction;

        static LoggerExtensionsForApplication()
        {
            ApplicationStartedAction = LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(ApplicationStarted)), $"Application started on {DateTime.Now}");

            ApplicationExitedAction = LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(ApplicationStarted)), $"Application exited on {DateTime.Now}");

            ApplicationFaultedAction = LoggerMessage.Define(LogLevel.Error, new EventId(1, nameof(ApplicationStarted)), $"Application faulted on {DateTime.Now}");
        }

        public static void ApplicationStarted(this ILogger logger)
        {
            ApplicationStartedAction(logger, null);
        }

        public static void ApplicationExited(this ILogger logger)
        {
            ApplicationExitedAction(logger, null);
        }

        public static void ApplicationFaulted(this ILogger logger, Exception ex)
        {
            ApplicationFaultedAction(logger, ex);
        }
    }
}
