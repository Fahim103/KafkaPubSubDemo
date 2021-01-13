using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleConsumerFirstDemo
{
    class Program
    {
        private static ServiceProvider ServiceProvider;
        public static IConfiguration Configuration { get; private set; }
        public static readonly string TopicName = "demo";

        static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("AspNetConsole_Environment");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true);

            Configuration = builder.Build();

            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton<IHostedService, KafkaConsumerFirstDemo>();

            ServiceProvider = _serviceCollection.BuildServiceProvider();

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<KafkaConsumerFirstDemo>();
                });
    }
}
