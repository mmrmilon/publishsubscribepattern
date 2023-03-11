namespace Pub.ResourceEngagement.Services
{
    public interface IPublisherService
    {
        void PublishMessage<T>(T message, string exchangeName, string exchangeType, string routeKey);
    }
}
