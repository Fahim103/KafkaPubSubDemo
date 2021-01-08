using Autofac;
using NHibernate;

namespace PublisherDemo.Web
{
    public class ModuleBindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(s => NHibernateDbContext.GetSession()).As<ISession>().InstancePerRequest();

            base.Load(builder);
        }
    }
}