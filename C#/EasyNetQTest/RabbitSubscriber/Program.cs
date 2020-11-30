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
            
            //channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            //var queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(queue: "TeunsQueue",
            //                  exchange: "logs",
            //                  routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue: "TeunsQueue",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
