

namespace Palette.Application.Dtos;

// dto for order data
public class OrderDto
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Guid SellerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public long TotalAmount { get; set; }
    public string Currency { get; set; } = "PHP";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}