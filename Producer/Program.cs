using RabbitMQ.Client;
using System.Text;

internal class Program
{
    public static void Main(string[] args)
    {
        ConnectionFactory connectionFactory = new()
        {
            Uri = new Uri("amqps://xagvccqi:nlyN6nssoYYaAESqUZuu1G-2GsReIjEe@fly.rmq.cloudamqp.com/xagvccqi")
        };

        using IConnection connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        #region Queue Decleration
        //string queueName = "hello_rabbit_mq";
        //channel.QueueDeclare(queueName, true, false, false);
        #endregion
        //
        #region Fanout Exchange Decleration
        channel.ExchangeDeclare("logs.Fanout", ExchangeType.Fanout, true);
        #endregion
        //
        var message = Console.ReadLine();

        Enumerable.Range(1, 60).ToList().ForEach(x =>
        {
            var messageBytes = Encoding.UTF8.GetBytes(message + $"{x}");

            channel.BasicPublish("logs.Fanout", string.Empty, null, messageBytes);
        });
    }
}