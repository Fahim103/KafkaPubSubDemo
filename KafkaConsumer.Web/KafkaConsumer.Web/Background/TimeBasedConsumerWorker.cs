using Confluent.Kafka;
using KafkaConsumer.Web.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace KafkaConsumer.Web.Background
{
    public class TimeBasedConsumerWorker
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public TimeBasedConsumerWorker()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = Status.TimeBasedConsumerGroupID,
                EnableAutoOffsetStore = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void StartProcessing(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.WriteLine($"Timebased Consumer Subscriping to topic -> demo");
            _consumer.Subscribe("demo");

            var date = DateTime.UtcNow.ToLocalTime();
            var timeStampCollection = new List<TopicPartitionTimestamp>();

            foreach (var partition in _consumer.Assignment)
            {
                timeStampCollection.Add(new TopicPartitionTimestamp("demo", partition.Partition, new Timestamp(date)));
            }

            var topicPartitionOffset = _consumer.OffsetsForTimes(timeStampCollection, TimeSpan.FromSeconds(10));
            
            foreach(var tpo in topicPartitionOffset)
            {
                _consumer.Seek(tpo);
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                Debug.WriteLine($"Timebased Consumer Received Message : {message}");

                // Write in the global shared variable
                Status.TimeBasedMessage = message;
                Status.TimeBasedMessageList.Add(message);

                _consumer.Commit(consumeResult);
            }

            _consumer.Close();
        }
    }
}