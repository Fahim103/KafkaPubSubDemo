using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSendAgent
{
    public class SubscriptionDto
    {
        public string URI { get; set; }
        public string TopicName { get; set; }
        public string Message { get; set; }

    }
}
