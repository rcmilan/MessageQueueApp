using MassTransit;
using MessageQueueApp.Events;

namespace MessageQueueApp.Consumers
{
    public class TicketConsumer(ILogger<TicketConsumer> logger) : IConsumer<CreateTicketEvent>
    {
        public Task Consume(ConsumeContext<CreateTicketEvent> context)
        {
            var @event = context.Message;

            logger.LogInformation(@event.Description);

            return Task.CompletedTask;
        }
    }
}
