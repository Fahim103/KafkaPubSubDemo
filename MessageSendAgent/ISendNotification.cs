using System.Threading.Tasks;

namespace MessageSendAgent
{
    public interface ISendNotification
    {
        Task SendMessageAsync(SubscriptionDto subscriptionDto);
    }
}