using RabbitMQ.Client;
using System.Text;

internal class Program
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4,
    }
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
        //channel.ExchangeDeclare("logs.Fanout", ExchangeType.Fanout, true);

        //Console.WriteLine("Enter weather updates as 'City.WeatherType': ");

        //var message = Console.ReadLine();

        //Enumerable.Range(1, 5).ToList().ForEach(x =>
        //{
        //    var messageBytes = Encoding.UTF8.GetBytes(message + $"{x}");

        //    channel.BasicPublish("logs.Fanout", string.Empty, null, messageBytes);
        //});
        #endregion
        //
        #region Topic Exhcange Decleration 
        channel.ExchangeDeclare("news", ExchangeType.Topic, false, false);

        Console.WriteLine("Enter weather updates as 'City.WeatherType': ");

        while (true)
        {
            var routingKey = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(routingKey)) break;

            var body = Encoding.UTF8.GetBytes(routingKey);
            channel.BasicPublish("weather", routingKey, null, body);
            Console.WriteLine($"Sent weather update: {routingKey}");
        }

        #endregion


    }
}