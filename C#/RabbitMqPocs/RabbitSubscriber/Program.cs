using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitSubscriber
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string exchange = "orders";
            const string queue = exchange + ".mocksub";

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
            channel.QueueDeclare(queue);

            // Messages published to exchange, should be directed to our queue
            channel.QueueBind(
                queue: queue,
                exchange: exchange,
                routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                byte[] body = eventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [string] {0}", message);
            };
            channel.BasicConsume(
                queue: queue,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
