using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQSend
{
    class EmitLogDirect
    {
        public static void Run(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct", durable: true, autoDelete: false, arguments: null);

                var severity = (args.Length > 0) ? args[0] : "info";
                var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "direct_logs", routingKey: severity, basicProperties: basicProperties, body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
