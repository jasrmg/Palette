

namespace Palette.Application.Dtos;

// dto for conversation data
public class ConversationDto
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid? ListingId { get; set; }
    public DateTime LastMessageAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public List<MessageDto> Messages { get; set; } = new();

}