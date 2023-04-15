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

        channel.BasicQos(0, 20, false);

        string queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queueName, "logs.Fanout", string.Empty);


        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, deliveryEventsarg) =>
        {
            var messageBody = deliveryEventsarg.Body.ToArray();
            Thread.Sleep(2500);

            Console.WriteLine($"Message: {Encoding.UTF8.GetString(messageBody)}");
            channel.BasicAck(deliveryEventsarg.DeliveryTag, false);
        };

        channel.BasicConsume(queueName, false, consumer);
        Console.ReadLine();
    }
}