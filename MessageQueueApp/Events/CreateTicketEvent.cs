using MessageQueueApp.Shared;

namespace MessageQueueApp.Events
{
    public class CreateTicketEvent : BaseEvent
    {
        public CreateTicketEvent(string description) : base()
        {
            Description = description;
        }

        public string Description { get; }
    }
}
