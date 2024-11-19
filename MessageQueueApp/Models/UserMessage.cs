namespace MessageQueueApp.Models;

public class UserMessage
{
    public UserMessage(DateTime createdAt, string content)
    {
        CreatedAt = createdAt;
        Content = content;
    }

    public int Id { get; }
    public DateTime CreatedAt { get; }
    public string Content { get; }
}
