using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteControlService.Core
{
    public class Application : IApplication
    {
        private static IServiceProvider _provider;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellationToken;

        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            _provider = serviceCollection.AddLogging()
                                         .AddSingleton<IApplication, Application>()
                                         .BuildServiceProvider();

            return _provider;
        }

        public Application()
        {
            _logger = _provider.GetService<ILoggerFactory>().CreateLogger<Application>();
            _cancellationToken = new CancellationTokenSource();
        }

        public void Start()
        {
            _logger.LogInformation("Started running the WebApi ...");

            Task.Run(() =>
            {
                using (WebApp.Start<Startup>("http://localhost:12345"))
                {
                    while (!_cancellationToken.IsCancellationRequested)
                    {
                    }
                }

                _logger.LogInformation("Stopped");
            });
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }
    }
}
