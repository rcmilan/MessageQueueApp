using MessageQueueApp.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueApp.Consumers;

public class MessageConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
        var connection = await factory.CreateConnectionAsync(cancellationToken: stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: typeof(PostMessageRequest).Name,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var message = System.Text.Json.JsonSerializer.Deserialize<string>(contentString);

            Console.WriteLine(message);

            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(typeof(PostMessageRequest).Name, false, consumer, cancellationToken: stoppingToken);
    }
}
