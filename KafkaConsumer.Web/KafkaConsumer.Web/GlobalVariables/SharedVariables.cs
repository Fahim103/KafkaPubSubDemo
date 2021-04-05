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
        private static bool _isNewGroupID = false;

        public static string ConsumerGroupID = _consumerGroupId;


        public static string TimeBasedConsumerGroupID = Guid.NewGuid().ToString();
        public static string TimeBasedMessage = "Initital Dummy Message for timebased...";
        public static List<string> TimeBasedMessageList = new List<string>();
        public static readonly string DemoTopicName = "Demo";


        public static (string groupId, bool isNewGroupId) GetConsumerInfo()
        {
            if (string.IsNullOrWhiteSpace(_consumerGroupId))
            {
                (_consumerGroupId, _isNewGroupID) = ConsumerGroupIdAllocator.GetGroupId();
            }

            return (_consumerGroupId, _isNewGroupID);
        } 

        public static string GetConsumerGroupId()
        {
            return GetConsumerInfo().groupId;
        }

        //private static string GetConsumerGroupID()
        //{
        //    if(string.IsNullOrWhiteSpace(_consumerGroupId))
        //    {
        //        (_consumerGroupId, _isNewGroupID) = ConsumerGroupIdAllocator.GetGroupId();
        //    }

        //    return _consumerGroupId;
        //}
    }
}