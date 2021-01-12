using KafkaConsumer.Web.Background;
using System.Diagnostics;
using System.Threading;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KafkaConsumer.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly StatusUpdateWorker _statusUpdateWorker;
        private readonly TimeBasedConsumerWorker _timeStampUpdateWorker;
        private CancellationTokenSource StatusUpdateWorkerCancellationTokenSource;
        private CancellationTokenSource TimeBasedConsumerWorkerCancellationTokenSource;

        public WebApiApplication()
        {
            _statusUpdateWorker = new StatusUpdateWorker();
            _timeStampUpdateWorker = new TimeBasedConsumerWorker();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Debug.WriteLine($"Starting {nameof(StatusUpdateWorker)} for consuming messages from topic -> demo");

            //HostingEnvironment.QueueBackgroundWorkItem(
            //    cancellationToken => new StatusUpdateWorker().StartProcessing(cancellationToken));

            //HostingEnvironment.QueueBackgroundWorkItem(ct =>
            //{
            //    StatusUpdateWorkerCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
            //    var cancellationToken = StatusUpdateWorkerCancellationTokenSource.Token;

            //    Debug.WriteLine($"Token is {cancellationToken}");
            //    _statusUpdateWorker.StartProcessing(cancellationToken);
            //});

            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                TimeBasedConsumerWorkerCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                var cancellationToken = TimeBasedConsumerWorkerCancellationTokenSource.Token;

                Debug.WriteLine($"Token is {cancellationToken}");
                _timeStampUpdateWorker.StartProcessing(cancellationToken);
            });
        }


        protected void Application_End()
        {
            // Cancel the kafka consumer
            //StatusUpdateWorkerCancellationTokenSource.Cancel();
            TimeBasedConsumerWorkerCancellationTokenSource.Cancel();
        }
    }
}
