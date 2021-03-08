using System;
using System.Collections.Generic;

namespace KafkaConsumer.Web.GlobalVariables
{
    public class SharedVariables
    {
        public static string Message = "Initial Dummy Message...";
        public static List<string> MessagesList = new List<string>();
        //public static string ConsumerGroupID = "web-client-01";
        private static string _consumerGroupId = string.Empty;
        public static string ConsumerGroupID = GetConsumerGroupID();
        public static string TimeBasedConsumerGroupID = Guid.NewGuid().ToString();
        public static string TimeBasedMessage = "Initital Dummy Message for timebased...";
        public static List<string> TimeBasedMessageList = new List<string>();
        public static readonly string DemoTopicName = "Demo";


        private static string GetConsumerGroupID()
        {
            if(string.IsNullOrWhiteSpace(_consumerGroupId))
            {
                _consumerGroupId = ConsumerGroupIdAllocator.GetGroupId();
            }

            return _consumerGroupId;
        }
    }
}