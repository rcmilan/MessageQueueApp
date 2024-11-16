using MessageQueueApp.Database;
using MessageQueueApp.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueApp.Consumers;

public class MessageConsumer : BackgroundService
{

    private readonly IServiceScopeFactory scopeFactory;

    public MessageConsumer(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

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
            var message = System.Text.Json.JsonSerializer.Deserialize<PostMessageRequest>(contentString);

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MessageQueueAppDbContext>();

                dbContext.UserMessages.Add(new Models.UserMessage(DateTime.Now, message.Content));
                await dbContext.SaveChangesAsync(stoppingToken);
            }

            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(typeof(PostMessageRequest).Name, false, consumer, cancellationToken: stoppingToken);
    }
}
