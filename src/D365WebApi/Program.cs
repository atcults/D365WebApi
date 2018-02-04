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
                //.AddSingleton<IFooService, FooService>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            logger.LogDebug("All done!");
        }
    }
}
