using Confluent.Kafka;
using PublisherDemo.Web.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PublisherDemo.Web.Controllers
{
    public class MessagePublishController : Controller
    {
        [HttpGet]
        public ActionResult PublishMessage()
        {
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
                BootstrapServers = "localhost:9092"
            };

            var producer = new ProducerBuilder<Null, string>(config).Build();

            await producer.ProduceAsync(publishMessageModel.TopicName, new Message<Null, string>()
            {
                Value = publishMessageModel.Message
            }, CancellationToken.None);

            return RedirectToAction("PublishMessage");
        }
    }
}