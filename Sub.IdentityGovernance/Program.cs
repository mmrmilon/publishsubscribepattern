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
            string userName = config["PublisherSettings:UserName"]!.ToString();
            string password = config["PublisherSettings:Password"]!.ToString();
            string exchangeName = config["PublisherSettings:ExchangeName"]!.ToString();
            string exchangeType = config["PublisherSettings:ExchangeType"]!.ToString();
            string routingKey = config["PublisherSettings:RoutingKey"]!.ToString();
            string queueName = config["PublisherSettings:QueueName"]!.ToString();

            Console.WriteLine("Identity Governance Engine");
            Console.WriteLine("*******************************************************");

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, true, false, null);

            string queueDeclare;
            if (string.IsNullOrEmpty(routingKey))
            {
                queueDeclare = channel.QueueDeclare().QueueName;
            }
            else
            {
                queueDeclare = channel.QueueDeclare(queueName, durable: true, autoDelete: false, exclusive: false).QueueName;
            }

            channel.QueueBind(queue: queueDeclare, exchange: exchangeName, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received: {message}");
            };

            channel.BasicConsume(queue: queueDeclare, autoAck: true, consumer: consumer);

            Console.ReadKey();
        }
    }
}