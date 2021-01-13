using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleConsumerFirstDemo
{
    public class KafkaConsumerFirstDemo : IHostedService
    {
        private readonly IConsumer<string, string> _consumer;
        //private readonly string GroupId = Guid.NewGuid().ToString();

        public KafkaConsumerFirstDemo()
        {

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "console-demo1",
                AutoOffsetReset = AutoOffsetReset.Earliest,

                //EnableAutoCommit = false

                // To manually store offset after work
                EnableAutoCommit = true, // The default value as it is
                EnableAutoOffsetStore = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Process Id: {Process.GetCurrentProcess().Id}");
            Console.WriteLine($"Subscribing to topic {Program.TopicName}");
            _consumer.Subscribe(Program.TopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                Console.WriteLine($"Received Message with key: {consumeResult.Message.Key} AND value: {consumeResult.Message.Value}");

                // _consumer.Commit(consumeResult);

                // To manually store offset after work
                _consumer.StoreOffset(consumeResult);
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
