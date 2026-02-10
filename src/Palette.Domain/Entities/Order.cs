

namespace Palette.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public long TotalAmount { get; private set; }
    public string Currency { get; private set; } = "PHP";
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    // order items (what was purchased)
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    // create new order
    public Order(Guid buyerId, Guid sellerId, List<OrderItem> items)
    {
        if (buyerId == Guid.Empty) throw new ArgumentException
        ("Buyer ID cannot be empty");
        if (sellerId == Guid.Empty) throw new ArgumentException("Seller ID cannot be empty");
        if (items == null || items.Count == 0) throw new ArgumentException("Order must have at least one item");

        Id = Guid.NewGuid();
        BuyerId = buyerId;
        SellerId = sellerId;
        Status = OrderStatus.Pending;
        Currency = "PHP";
        CreatedAtUtc = DateTime.UtcNow;

        _items.AddRange(items);
        TotalAmount = items.Sum(i => i.LineTotal);
    }

    // confirm order (seller accepts)
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Can only confirm pending orders");

        Status = OrderStatus.Confirmed;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // complete order (delivered/finished)
    public void Complete()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Can only complete confirmed orders");

        Status = OrderStatus.Completed;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // cancel order
    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed orders");
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        Status = OrderStatus.Cancelled;
        UpdatedAtUtc = DateTime.UtcNow;
    }

}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3
}
