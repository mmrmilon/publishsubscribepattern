using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Sub.IdentityGovernance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Identity Governance Engine");
            Console.WriteLine("*******************************************************");

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "amq.fanout", type: ExchangeType.Fanout, true, false, null);
            var queueName = channel.QueueDeclare("EngagementOrderQueue_IG", durable: true, autoDelete: false, exclusive: false);
            
            // take 1 message per consumer
            //channel.BasicQos(0, 1, false);
            channel.QueueBind(queue: queueName,
                exchange: "amq.fanout",
                routingKey: "EngagementOrders");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received: {message}");
            };

            channel.BasicConsume(queue: "EngagementOrderQueue_IG", autoAck: true, consumer: consumer);

            Console.ReadKey();
        }
    }
}