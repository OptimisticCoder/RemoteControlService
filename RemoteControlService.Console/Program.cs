using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RemoteControlService.Core;

namespace RemoteControlService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Application.ConfigureServices();
            serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Information);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            var application = serviceProvider.GetService<IApplication>();

            application.Start();

            logger.LogInformation("Press any key to exit.");
            System.Console.ReadKey();

            application.Stop();
        }
    }
}
