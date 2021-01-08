using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PublisherDemo.Web.Models
{
	public class SubscriptionCreateModel
	{

		[Required]
		public string URI { get; set; }

		[Required]
		[Display(Name = "Topic Name")]
		public string TopicName { get; set; }
	}
}