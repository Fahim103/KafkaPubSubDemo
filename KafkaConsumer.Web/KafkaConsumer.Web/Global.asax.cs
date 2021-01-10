using KafkaConsumer.Web.Background;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KafkaConsumer.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Debug.WriteLine($"Starting {nameof(StatusUpdateWorker)} for consuming messages from topic -> demo");

            HostingEnvironment.QueueBackgroundWorkItem(
                cancellationToken => new StatusUpdateWorker().StartProcessing(cancellationToken));
        }
    }
}
