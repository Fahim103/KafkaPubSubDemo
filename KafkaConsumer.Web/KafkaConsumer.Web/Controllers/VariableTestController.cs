using KafkaConsumer.Web.Background;
using KafkaConsumer.Web.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace KafkaConsumer.Web.Controllers
{
    public class VariableModel
    {
        public string Message { get; set; }
    }
    public class VariableTestController : Controller
    {
        // GET: VariableTest
        public ActionResult Index()
        {
            Debug.WriteLine($"Starting {nameof(StatusUpdateWorker)} for consuming messages");

            HostingEnvironment.QueueBackgroundWorkItem(
                cancellationToken => new StatusUpdateWorker().StartProcessing(cancellationToken));

            var model = new VariableModel
            {
                Message = Status.Message
            };


            return View(model);
        }
    }
}