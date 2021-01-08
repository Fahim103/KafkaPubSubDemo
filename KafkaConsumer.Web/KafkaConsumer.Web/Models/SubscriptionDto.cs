using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KafkaConsumer.Web.Models
{
    public class SubscriptionDto
    {
        public string URI { get; set; }
        public string TopicName { get; set; }
        public string Message { get; set; }

    }
}