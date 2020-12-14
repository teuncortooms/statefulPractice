using System;
using EasyNetQ;
using Messages;

namespace Publisher
{
    class Program
    {
        static void Main()
        {
            using var bus = RabbitHutch.CreateBus("host=localhost");
            var input = "";
            Console.WriteLine("Enter a message. 'Quit' to quit.");
            while ((input = Console.ReadLine()) != "Quit")
            {
                bus.PubSub.Publish(new OrderMessage
                {
                    Text = input
                });
            }
        }
    }
}