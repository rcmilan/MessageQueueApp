namespace MessageQueueApp.IO;

public record GetMessageResponse(int Id, DateTime CreatedAt, string Content);
