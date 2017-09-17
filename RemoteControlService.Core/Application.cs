using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace RemoteControlService.Core
{
    public class Application : IApplication
    {
        private static IContainer _container;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellationToken;
        private IDisposable _webApi;
        private int _counter = 0;

        public static IServiceProvider ConfigureServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Populate(new ServiceCollection().AddLogging()
                                                    .AddSingleton<IApplication, Application>());
            _container = builder.Build();

            return _container.Resolve<IServiceProvider>();
        }

        public Application()
        {
            _logger = _container.Resolve<ILoggerFactory>().CreateLogger<Application>();

            _cancellationToken = new CancellationTokenSource();
            _cancellationToken.Token.Register(() =>
            {
                _webApi.Dispose();
                _logger.LogInformation("Stopped");
            });
        }

        public void Start()
        {
            _webApi = WebApp.Start("http://localhost:12345", (appBuilder) =>
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();

                config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                appBuilder.UseWebApi(config);
            });

            _logger.LogInformation("Started");

            Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    ++_counter;
                }
            });
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public int GetCount()
        {
            _logger.LogInformation("Count was requested");
            return _counter;
        }
    }
}
