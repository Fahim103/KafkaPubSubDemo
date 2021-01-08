using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using PublisherDemo.Web.Convention;
using PublisherDemo.Web.Mappings;
using System.Configuration;

namespace PublisherDemo.Web
{
    public class NHibernateDbContext
    {
        private static ISessionFactory _session;

        private static ISessionFactory CreateSessionFactory()
        {
            if (_session != null)
            {
                return _session;
            }

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            FluentConfiguration _config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.Conventions.AddFromAssemblyOf<TableNameConvention>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SubscriptionMapping>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.SessionFactory().DefaultFlushMode(FlushMode.Commit);
                    new SchemaUpdate(cfg).Execute(false, true);
                });

            _session = _config.BuildSessionFactory();

            return _session;
        }

        public static ISession GetSession()
        {
            return CreateSessionFactory().OpenSession();
        }
    }
}