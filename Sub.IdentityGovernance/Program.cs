using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Sub.IdentityGovernance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var config = builder.Build();
            string hostName = config["PublisherSettings:HostName"]!.ToString();
            string exchangeName = config["PublisherSettings:ExchangeName"]!.ToString();
            string exchangeType = config["PublisherSettings:ExchangeType"]!.ToString();
            string routeKey = config["PublisherSettings:RouteKey"]!.ToString();
            string queueName = config["PublisherSettings:QueueName"]!.ToString();

            Console.WriteLine("Identity Governance Engine");
            Console.WriteLine("*******************************************************");

            var factory = new ConnectionFactory
            {
                HostName = hostName
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, true, false, null);
            var queueDeclare = channel.QueueDeclare(queueName, durable: true, autoDelete: false, exclusive: false);

            channel.QueueBind(queue: queueDeclare, exchange: exchangeName, routingKey: routeKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.ReadKey();
        }
    }
}