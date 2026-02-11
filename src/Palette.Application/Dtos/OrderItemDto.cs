


namespace Palette.Application.Dtos;

// dto for item data
public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string TitleSnapshot { get; set; } = string.Empty;
    public long UnitPriceSnapshot { get; set; }
    public int Quantity { get; set; }
    public long LineTotal { get; set; }
}