

namespace Palette.Domain.Entities;


// represents a single item in an order with price snapshot
public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ListingId { get; private set; }
    public string TitleSnapshot { get; private set; } = string.Empty;
    public long UnitPriceSnapshot { get; private set; }
    public int Quantity { get; private set; }
    public long LineTotal { get; private set; }

    private OrderItem() { }

    // create new order item with price snapshot
    public OrderItem(Guid listingId, string titleSnapshot, long unitPriceSnapshot, int quantity)
    {
        if (listingId == Guid.Empty) throw new ArgumentException("Listing ID cannot be empty");
        if (string.IsNullOrWhiteSpace(titleSnapshot)) throw new ArgumentException("Title Snapshot cannot be empty");
        if (unitPriceSnapshot <= 0) throw new ArgumentException("Unit price must be greater than zero");
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero");

        Id = Guid.NewGuid();
        ListingId = listingId;
        TitleSnapshot = titleSnapshot;
        UnitPriceSnapshot = unitPriceSnapshot;
        Quantity = quantity;
        LineTotal = unitPriceSnapshot * quantity; // calculate line total
    }
}