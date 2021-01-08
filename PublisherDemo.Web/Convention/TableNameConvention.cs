using FluentNHibernate.Conventions;
using Pluralize.NET;

namespace PublisherDemo.Web.Convention
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            string typeName = instance.EntityType.Name;

            IPluralize pluralizer = new Pluralizer();

            instance.Table(pluralizer.Pluralize(typeName));
        }
    }
}