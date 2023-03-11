using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pub.ResourceEngagement.Options;
using RabbitMQ.Client;
using System.Text;

namespace Pub.ResourceEngagement.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly ILogger<PublisherService> _logger;
        private readonly PublisherOptions _publisherOptions;
        private readonly IConnection _connection;
        public PublisherService(ILogger<PublisherService> logger, IOptions<PublisherOptions> options)
        {
            _logger = logger;
            _publisherOptions = options.Value;
            //_connection = GetConnection();
        }

        public void PublishMessage<T>(T message, string exchangeName, string exchangeType, string routeKey)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _publisherOptions.HostName
                };

                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                //using var channel = _connection.CreateModel();

                //channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                channel.QueueDeclare("EngagementOrders", exclusive: false);

                var jsonMessage = JsonConvert.SerializeObject(message);
                var byteMessage = Encoding.UTF8.GetBytes(jsonMessage);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, body: byteMessage);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _publisherOptions.HostName,
                UserName = _publisherOptions.UserName,
                Password = _publisherOptions.Password,
                Port = _publisherOptions.Port
            };

            return factory.CreateConnection();
        }
    }
}
