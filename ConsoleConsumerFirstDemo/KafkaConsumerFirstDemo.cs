using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleConsumerFirstDemo
{
    public class KafkaConsumerFirstDemo : IHostedService
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumerFirstDemo()
        {

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "console-demo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            _consumer.Subscribe("demo");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                Console.WriteLine($"Received Message : {consumeResult.Message.Value}");

            }

            _consumer.Close();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Close();
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
