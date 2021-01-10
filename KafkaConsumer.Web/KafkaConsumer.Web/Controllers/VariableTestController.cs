﻿using KafkaConsumer.Web.GlobalVariables;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KafkaConsumer.Web.Controllers
{
    public class VariableModel
    {
        public string Message { get; set; }
        public List<string> MessageList { get; set; }

        public VariableModel()
        {
            MessageList = Status.MessagesList;
        }
    }
    public class VariableTestController : Controller
    {
        // GET: VariableTest
        public ActionResult Index()
        {
            //Debug.WriteLine($"Starting {nameof(StatusUpdateWorker)} for consuming messages");

            //HostingEnvironment.QueueBackgroundWorkItem(
            //    cancellationToken => new StatusUpdateWorker().StartProcessing(cancellationToken));

            var model = new VariableModel
            {
                Message = Status.Message
            };


            return View(model);
        }
    }
}