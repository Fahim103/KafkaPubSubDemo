using KafkaConsumer.Web.GlobalVariables;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace KafkaConsumer.Hubs
{
    [HubName("testMessageHub")]
    public class TestMessageHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestMessageHub>();
            context.Clients.All.refreshKafkaData(SharedVariables.Message);
        }
    }
}