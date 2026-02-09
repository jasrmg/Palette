

namespace Palette.Application.Dtos;


// dto for returning listing data to api clients
public class ListingDto
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long PriceAmount { get; set; }
    public string Currency { get; set; } = "PHP";
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}