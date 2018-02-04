using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


/*
 * CREDIT: https://github.com/andrewlock/blog-examples/tree/master/using-dependency-injection-in-a-net-core-console-application
 */


namespace D365WebApi
{

    class Program
    {
        static void Main(string[] args)
        {
            //    Uncomment to use the built in container
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug)
                .AddDebug(LogLevel.Trace);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Starting application");

            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "D365Client");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            var filePath = Path.Combine(appDataFolder, "cloudkestrel-prod.json");

            File.WriteAllText(filePath, "test");

            Console.WriteLine("Press any key to exit!");

            Console.ReadLine();
        }
    }
}
