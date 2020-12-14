using System;
using EasyNetQ;
using Messages;

namespace Subscriber
{
    class Program
    {
        static void Main()
        {
            using var bus = RabbitHutch.CreateBus("host=localhost");
            string id = Guid.NewGuid().ToString();
            bus.PubSub.Subscribe<OrderMessage>("test" + id, HandleTextMessage);

            Console.WriteLine("Listening for messages. Hit <return> to quit.");
            Console.ReadLine();
        }

        static void HandleTextMessage(OrderMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", textMessage.Text);
            Console.ResetColor();
        }
    }
}