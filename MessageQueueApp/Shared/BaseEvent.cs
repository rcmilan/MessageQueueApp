namespace MessageQueueApp.Shared
{
    public abstract class BaseEvent
    {
        public BaseEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
    }
}
