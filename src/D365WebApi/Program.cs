using System;
using System.IO;
using D365WebApi.Core;
using D365WebApi.LogExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


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

            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "D365Client");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            var filePath = Path.Combine(appDataFolder, "cloudkestrel-prod.json");

            if (!File.Exists(filePath))
            {
                using (var file = File.CreateText(filePath))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, new ClientConfiguration
                    {
                        ClientId = "--YOUR CLIENT ID--",
                        ClientSecret = "--YOUR CLIENT SECRET--",
                        Region = "--YOUR REGION NAME--",
                        TenantId = "--YOUR TENANT ID--",
                        InstanceName = "--YOUR INSTANCE NAME--"
                    });
                }
            }

            Console.WriteLine("Press any key to exit!");

            Console.ReadLine();
        }
    }
}
