using System;
using D365WebApi.LogExtensions;
using D365WebApi.Services;
using D365WebApi.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


/*
 * CREDITS: 
 * DI: https://github.com/andrewlock/blog-examples/tree/master/using-dependency-injection-in-a-net-core-console-application
 * Logging: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage
 * 
 */


namespace D365WebApi
{

    class Program
    {
        static void Main(string[] args)
        {
            //    Uncomment to use the built in container
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddFilter("Default", LogLevel.Warning);
                    builder.AddFilter("System", LogLevel.Information);
                    builder.AddFilter("Microsoft", LogLevel.Information);
                })
                .AddSingleton<IClientConfigurationProvider, ClientConfigurationProvider>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug)
                .AddDebug(LogLevel.Trace);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.ApplicationStarted();

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                logger.ApplicationFaulted((Exception) eventArgs.ExceptionObject);
            };

            var configProvider = serviceProvider.GetService<IClientConfigurationProvider>();

            var config = configProvider.ReadFromDisk("cloudkestrel-prod");

            logger.LogInformation(config.Region);

            Console.WriteLine("Press any key to exit!");

            Console.ReadLine();
            logger.ApplicationExited();
        }
    }
}
