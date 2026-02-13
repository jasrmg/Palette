

namespace Palette.Domain.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public Guid ConversationId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Body { get; private set; } = string.Empty;
    public DateTime SentAtUtc { get; private set; }

    private Message() { }

    // create new message
    public Message(Guid conversationId, Guid senderId, string body)
    {
        if (conversationId == Guid.Empty) throw new ArgumentException("Conversation ID cannot be empty");
        if (senderId == Guid.Empty) throw new ArgumentException("Sender ID cannot be empty");
        if (string.IsNullOrWhiteSpace(body)) throw new ArgumentException("Message body cannot be empty");

        Id = Guid.NewGuid();
        ConversationId = conversationId;
        SenderId = senderId;
        Body = body;
        SentAtUtc = DateTime.UtcNow;
    }
}