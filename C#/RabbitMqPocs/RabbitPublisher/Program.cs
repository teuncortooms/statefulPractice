using System;
using System.Text;
using System.Threading.Tasks;
using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitPublisher
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            const string exchange = "orders";

            try
            {
                var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
                using var rabbitConnection = connectionFactory.CreateConnection();
                using var channel = rabbitConnection.CreateModel();

                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

                // optional properties
                var props = channel.CreateBasicProperties();
                props.AppId = "MOCK";
                props.Persistent = true;
                // props.UserId = "ops0";
                props.MessageId = Guid.NewGuid().ToString("N");
                props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                int counter = 0;
                while (true)
                {
                    // body
                    var message = new OrderMessage { Text = $"A nice random message: {counter++}" };
                    var msgJson = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(msgJson);

                    // publish
                    channel.BasicPublish(
                        exchange: exchange,
                        routingKey: "", // ignored with type fanout
                        basicProperties: props,
                        body: body);

                    Console.WriteLine(" [x] Sent: {0}", msgJson);

                    await Task.Delay(2000);
                }

                ;
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