namespace Pub.ResourceEngagement.Services
{
    public interface IPublisherService
    {
        void PublishMessage<T>(T message);
    }
}
