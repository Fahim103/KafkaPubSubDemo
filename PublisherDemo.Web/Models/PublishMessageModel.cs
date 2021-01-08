using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PublisherDemo.Web.Models
{
    public class PublishMessageModel
    {
        [Required]
        [Display(Name = "Topic Name")]
        public string TopicName { get; set; }

        [Required]
        public string Message { get; set; }
    }
}