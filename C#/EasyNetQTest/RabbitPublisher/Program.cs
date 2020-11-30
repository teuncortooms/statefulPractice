using System;
using System.Text;
using System.Threading.Tasks;
using Messages;
using RabbitMQ.Client;

namespace RabbitPublisher
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            const string queueName = "TeunsQueue";

            try
            {
                var connectionFactory = new ConnectionFactory(){ HostName = "localhost" };
                using var rabbitConnection = connectionFactory.CreateConnection();
                using var channel = rabbitConnection.CreateModel();
                
                // Declaring a queue 
                channel.QueueDeclare((
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                // Or declare exchange?
                //channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                while (true)
                {
                    string body = $"A nice random message: {DateTime.Now.Ticks}";
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: queueName, // or use exchange "logs"
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(body));

                    Console.WriteLine("Message sent: " + body);
                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();
        }
    }
}