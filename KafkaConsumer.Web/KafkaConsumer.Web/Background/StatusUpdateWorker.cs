using Confluent.Kafka;
using KafkaConsumer.Web.GlobalVariables;
using System.Diagnostics;
using System.Threading;

namespace KafkaConsumer.Web.Background
{
    public class StatusUpdateWorker
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public StatusUpdateWorker()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = SharedVariables.ConsumerGroupID,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void StartProcessing(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.WriteLine($"Subscriping to topic -> {SharedVariables.DemoTopicName}");
            _consumer.Subscribe(SharedVariables.DemoTopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                var workerName = nameof(StatusUpdateWorker);
                Debug.WriteLine($"{workerName} Received Message : {message}");

                // Write in the global shared variable
                SharedVariables.Message = message;
                SharedVariables.MessagesList.Add(message);
            }

            _consumer.Close();
        }
    }
}