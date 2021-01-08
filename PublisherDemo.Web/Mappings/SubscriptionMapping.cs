using FluentNHibernate.Mapping;
using PublisherDemo.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PublisherDemo.Web.Mappings
{
    public class SubscriptionMapping : ClassMap<Subscription>
    {
        public SubscriptionMapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.TopicName).Not.Nullable();
            Map(s => s.URI).Not.Nullable();
        }
    }
}