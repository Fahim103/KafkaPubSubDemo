using Confluent.Kafka;
using PublisherDemo.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PublisherDemo.Web.Controllers
{
    public class MessagePublishController : Controller
    {
        private readonly static List<string> keys = new List<string>()
        {
            "First",
            "Second",
            "Third",
            "Fourth"
        };

        [HttpGet]
        public ActionResult PublishMessage()
        {
            ViewBag.ProcessId = Process.GetCurrentProcess().Id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PublishMessage(PublishMessageModel publishMessageModel)
        {
            if (!ModelState.IsValid)
            {
                return View(publishMessageModel);
            }

            // Send Message to Kafka
            // Default Port of Kafka is 9092
            
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092, localhost:9093, localhost:9094, localhost:9095"
            };

            var producer = new ProducerBuilder<string, string>(config).Build();
            var random = new Random();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var index = random.Next(0, 4);

                var deliveryReport = await producer.ProduceAsync(publishMessageModel.TopicName, new Message<string, string>()
                {
                    Key = keys[index],
                    Value = publishMessageModel.Message + " -> " + i
                }, CancellationToken.None);

                //tasks.Add(
                //    producer.ProduceAsync(publishMessageModel.TopicName, new Message<string, string>()
                //    {
                //        Key = keys[index],
                //        Value = publishMessageModel.Message + " -> " + i
                //    }, CancellationToken.None)
                //);
            }

            //Task.WaitAll(tasks.ToArray());
            
            return RedirectToAction("PublishMessage");
        }
    }
}