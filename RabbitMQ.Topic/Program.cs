using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        ConnectionFactory connectionFactory = new()
        {
            Uri = new("amqps://xagvccqi:nlyN6nssoYYaAESqUZuu1G-2GsReIjEe@fly.rmq.cloudamqp.com/xagvccqi")
        };

        using IConnection connection = connectionFactory.CreateConnection();
        using IModel channel = connection.CreateModel();

        channel.ExchangeDeclare("weather", ExchangeType.Topic);
        var queueName = channel.QueueDeclare().QueueName;

        Console.WriteLine("Enter binding keys (e.g. 'City.WeatherType', '*.sunny', 'City.*'): ");
        var bindingKeys = Console.ReadLine().Split(' ');

        foreach (var bindingKey in bindingKeys)
        {
            channel.QueueBind(queue: queueName, exchange: "weather", routingKey: bindingKey);
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            Console.WriteLine($"Received weather update: '{message}' with routing key: '{routingKey}'");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}