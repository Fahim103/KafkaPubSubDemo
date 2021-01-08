using System.ComponentModel.DataAnnotations;

namespace PublisherDemo.Web.Entities
{
	public class Subscription
	{
		[Required]
		public virtual int Id { get; set; }

		[Required]
		public virtual string URI { get; set; }
		
		[Required]
		public virtual string TopicName { get; set; }
	}
}