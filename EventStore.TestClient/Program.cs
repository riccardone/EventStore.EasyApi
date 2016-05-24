using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventStore.EasyApi;

namespace EventStore.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = EasyClient.CreateBus();
            Console.WriteLine("Press P to publish a message or S to subscribe for messages");
            var first = Console.ReadKey().Key;
            if (first == ConsoleKey.P)
            {
                while (true)
                {
                    Console.WriteLine("Enter the name:");
                    var name = Console.ReadLine();
                    var msg = new Person { Name = name };
                    bus.Publish(msg);
                    Console.WriteLine("Press A to send another message or B to exit the program");
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.B)
                    {
                        break;
                    }
                }
            }
            if (first == ConsoleKey.S)
            {
                bus.Subscribe<Person>("test", msg => Console.WriteLine(msg.Name));
                Console.WriteLine("waiting for messages... Press B to exit the program");
                while (true)
                {
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.B)
                    {
                        break;
                    }
                }
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
