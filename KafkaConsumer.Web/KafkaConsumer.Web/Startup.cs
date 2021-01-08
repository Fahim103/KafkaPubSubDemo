using Autofac;
using Autofac.Integration.Mvc;
using Serilog;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace KafkaConsumer.Web
{
    public class Startup
    {
        public static ILifetimeScope AutofacContainer { get; private set; }

        public static void ConfigureDepencyInjectionAndLogger()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(HttpApplication).Assembly);


            builder.Register<ILogger>((componentContext, parameters) =>
            {
                var test = componentContext;
                var param = parameters;
                var logPath = HostingEnvironment.MapPath("~/Logs/log-.txt");
                return new LoggerConfiguration()
                    .Enrich.FromLogContext().WriteTo.Console().CreateLogger();

                    //.File(logPath, rollingInterval: RollingInterval.Day,
                    //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
                    //.CreateLogger();
            }).SingleInstance();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AutofacContainer = container;
        }
    }
}