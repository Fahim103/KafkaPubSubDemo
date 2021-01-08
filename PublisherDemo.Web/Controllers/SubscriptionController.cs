using PublisherDemo.Web.Entities;
using PublisherDemo.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PublisherDemo.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        public SubscriptionController()
        {
            
        }

        [HttpGet]
        public ActionResult GetSubscriptions()
        {
            using (var session = NHibernateDbContext.GetSession())
            {
                return Json(session.Query<Subscription>().ToList());
            }
        }

        [HttpGet]
        public ActionResult CreateSubscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSubscription(SubscriptionCreateModel subscriptionCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(subscriptionCreateModel);
            }

            using (var session = NHibernateDbContext.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var subscription = new Subscription()
                        {
                            URI = subscriptionCreateModel.URI,
                            TopicName = subscriptionCreateModel.TopicName
                        };

                        session.Save(subscription);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!transaction.WasCommitted)
                        {
                            transaction.Rollback();
                        }

                        throw new Exception("Failed to create Subscription: " + ex.Message);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}