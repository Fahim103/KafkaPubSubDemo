using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace MessageSendAgent
{
    class Program
    {
        public static ServiceProvider ServiceProvider;
        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("AspNetConsole_Environment");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true);

            Configuration = builder.Build();

            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton<ISendNotification, SendNotification>();
            _serviceCollection.AddHttpClient();

            ServiceProvider = _serviceCollection.BuildServiceProvider();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<App>();
                });
    }
}
