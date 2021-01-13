using System;
using System.Collections.Generic;

namespace KafkaConsumer.Web.GlobalVariables
{
    public class SharedVariables
    {
        public static string Message = "Initial Dummy Message...";
        public static List<string> MessagesList = new List<string>();
        public static string ConsumerGroupID = Guid.NewGuid().ToString();
        public static string TimeBasedConsumerGroupID = Guid.NewGuid().ToString();
        public static string TimeBasedMessage = "Initital Dummy Message for timebased...";
        public static List<string> TimeBasedMessageList = new List<string>();
        public static readonly string DemoTopicName = "demo";
    }
}