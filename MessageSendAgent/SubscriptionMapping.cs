using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSendAgent
{
    public class SubscriptionMapping : ClassMap<Subscription>
    {
        public SubscriptionMapping()
        {
            Table("Subscriptions");
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.TopicName).Not.Nullable();
            Map(s => s.URI).Not.Nullable();
        }
    }
}
