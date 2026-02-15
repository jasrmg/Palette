
namespace Palette.Application.Dtos;

// dto for message data
public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime SentAtUtc { get; set; }
}