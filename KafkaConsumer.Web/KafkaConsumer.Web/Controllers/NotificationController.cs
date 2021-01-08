using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using KafkaConsumer.Web.Models;
using Serilog;
using Serilog.Core;

namespace KafkaConsumer.Web.Controllers
{
    public class NotificationController : ApiController
    {

        [Route("api/Notification/MessageReceived")]
        [HttpPost]
        public IHttpActionResult MessageReceived(SubscriptionDto subscriptionDto)
        {
            Log.Logger.Information($"Message Received -> {subscriptionDto.Message}");
            Debug.WriteLine($"Message Received -> {subscriptionDto.Message}");
            return Ok(subscriptionDto.Message);
        }
    }
}
