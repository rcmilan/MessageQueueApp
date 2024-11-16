namespace MessageQueueApp.Models
{
    public class UserMessage
    {
        public UserMessage(DateTime? createdAt, string content)
        {
            CreatedAt = createdAt ?? DateTime.Now;
            Content = content;
        }

        public int Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Content { get; private set; }
    }
}
