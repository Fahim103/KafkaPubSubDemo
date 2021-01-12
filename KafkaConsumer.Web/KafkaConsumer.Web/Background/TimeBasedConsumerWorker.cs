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
                GroupId = "timestamp-test",
                EnableAutoOffsetStore = false,
                //AutoCommitIntervalMs = 1000,
                //EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void StartProcessing(CancellationToken cancellationToken = default(CancellationToken))
        {
            //var date = new DateTime();
            var date = new DateTime(2021, 1, 12, 16, 13, 0);
            var timeStampCollection = new List<TopicPartitionTimestamp>();
            timeStampCollection.Add(new TopicPartitionTimestamp("demo", new Partition(0), new Timestamp(date)));
            timeStampCollection.Add(new TopicPartitionTimestamp("demo", new Partition(1), new Timestamp(date)));
            timeStampCollection.Add(new TopicPartitionTimestamp("demo", new Partition(2), new Timestamp(date)));


            Debug.WriteLine($"Subscriping to topic -> demo");
            _consumer.Subscribe("demo");
            var topicPartitionOffset = _consumer.OffsetsForTimes(timeStampCollection, TimeSpan.FromSeconds(10));
            //var topicPartitionOffsetToCommit = new List<TopicPartitionOffset>();

            //foreach (var tp in topicPartitionOffset)
            //{
            //    if (tp.Offset.Value != -1)
            //    {
            //        _consumer.StoreOffset(tp);
            //        topicPartitionOffsetToCommit.Add(tp);
            //    }
            //    else
            //    {
            //        var result = _consumer.QueryWatermarkOffsets(tp.TopicPartition, TimeSpan.FromSeconds(60));
            //        var tpOffset = new TopicPartitionOffset(tp.TopicPartition, result.High);
            //        _consumer.StoreOffset(tpOffset);
            //        topicPartitionOffsetToCommit.Add(tpOffset);
            //    }
            //}
            //_consumer.Commit(topicPartitionOffsetToCommit);
            //Thread.Sleep(10000); // To let kafka commit the offset
            //var commitOffset = _consumer.Committed(TimeSpan.FromSeconds(10));
            //while (_consumer.Position(topicPartitionOffset[0].TopicPartition) != commitOffset[0].Offset) ;
            //while (_consumer.Position(topicPartitionOffset[1].TopicPartition) != commitOffset[1].Offset) ;
            //while (_consumer.Position(topicPartitionOffset[2].TopicPartition) != commitOffset[2].Offset) ;
            
            _consumer.Seek(topicPartitionOffset[0]);
            _consumer.Seek(topicPartitionOffset[1]);
            _consumer.Seek(topicPartitionOffset[2]);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                Debug.WriteLine($"Received Message : {message}");

                // Write in the global shared variable
                Status.Message = message;
                Status.MessagesList.Add(message);

                _consumer.Commit(consumeResult);
            }

            _consumer.Close();
        }
    }
}