using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace RabbitMQSend
{
    class Program
    {
        static void Main(string[] args)
        {
            //first example
            //HelloWorld();

            //WorkQueues();

            EmitLog.Run(args);
        }

        private static void WorkQueues()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                    var message = "message...";
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    
                    channel.BasicPublish(exchange: "",
                                         routingKey: "task_queue",
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
      
        private static void HelloWorld()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
