using Confluent.Kafka;
using KafkaConsumer.Hubs;
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
            var (groupId, isNewGroupId) = SharedVariables.GetConsumerInfo();

            var config = new ConsumerConfig
            {
                //BootstrapServers = "192.168.2.63:9092, 192.168.3.17:9092",
                BootstrapServers = "localhost:9092",
                GroupId = groupId,
                AutoOffsetReset = isNewGroupId ? AutoOffsetReset.Latest : AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void StartProcessing(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.WriteLine($"Subscriping to topic -> {SharedVariables.DemoTopicName}");
            _consumer.Subscribe(SharedVariables.DemoTopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var message = consumeResult.Message.Value;
                    var workerName = nameof(StatusUpdateWorker);
                    Debug.WriteLine($"{workerName} Received Message : {message}");
                    // Write in the global shared variable
                    SharedVariables.Message = message;
                    SharedVariables.MessagesList.Add(message);
                    // call SignalR method
                    TestMessageHub.BroadcastData();
                }
                catch (System.Exception ex)
                {

                }
                
            }

            _consumer.Close();
        }
    }
}