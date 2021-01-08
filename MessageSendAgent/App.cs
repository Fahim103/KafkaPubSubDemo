using Confluent.Kafka;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageSendAgent
{
    public class App : IHostedService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private IList<Subscription> _subscriptions;
        private ISendNotification _sendNotification;

        public App()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "console-demo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            _sendNotification = Program.ServiceProvider.GetService<ISendNotification>();
        }

        private static ISessionFactory _session;

        private static ISessionFactory CreateSessionFactory()
        {
            if (_session != null)
            {
                return _session;
            }

            var connectionString = @"Server=.\SQLEXPRESS;Database=KafkaDemo;User Id=demo;Password=123456;";

            FluentConfiguration _config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SubscriptionMapping>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.SessionFactory().DefaultFlushMode(FlushMode.Commit);
                    new SchemaUpdate(cfg).Execute(false, true);
                });

            _session = _config.BuildSessionFactory();

            return _session;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting up....");

            _consumer.Subscribe("demo");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                var message = consumeResult.Message.Value;

                Console.WriteLine($"Received Message : {message}");

                // Get Subscription List from DB
                using (var currentSession = CreateSessionFactory().OpenSession())
                {
                    Console.WriteLine("Getting Subscriptions");
                    try
                    {
                        _subscriptions = currentSession.Query<Subscription>().ToList();
                        if (_subscriptions == null)
                        {
                            Console.WriteLine("Error! No subscriptions found....");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Debug.WriteLine(ex);
                    }
                    
                }

                foreach(var subscription in _subscriptions)
                {
                    Console.WriteLine("Sending Message");
                    var subscriptionDto = new SubscriptionDto
                    {
                        URI = subscription.URI,
                        TopicName = subscription.TopicName,
                        Message = message
                    };

                    await _sendNotification.SendMessageAsync(subscriptionDto);
                }
            }

            _consumer.Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Close();
            _consumer?.Dispose();

            return Task.CompletedTask;
        }
    }
}
