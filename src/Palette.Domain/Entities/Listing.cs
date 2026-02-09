

namespace Palette.Domain.Entities;

public class Listing
{
    public Guid Id { get; private set; }
    public Guid SellerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public long PriceAmount { get; private set; }
    public string Currency { get; private set; } = "PHP";
    public int Quantity { get; private set; }
    public ListingStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Listing() { }

    // create new listing
    public Listing(Guid sellerId, string title, string description, long priceAmount, int quantity)
    {
        if (sellerId == Guid.Empty) throw new ArgumentException("Seller ID cannot be empty");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty");
        if (priceAmount <= 0) throw new ArgumentException("Price must be greater than zero");
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative");

        Id = Guid.NewGuid();
        SellerId = sellerId;
        Title = title;
        Description = description ?? string.Empty;
        PriceAmount = priceAmount;
        Currency = "PHP";
        Quantity = quantity;
        Status = ListingStatus.Draft;
        CreatedAtUtc = DateTime.UtcNow;
    }

    // update listing details
    public void Update(string title, string description, long priceAmount, int quantity)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty");
        if (priceAmount <= 0) throw new ArgumentException("Price must be greater than zero");
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative");

        Title = title;
        Description = description ?? string.Empty;
        PriceAmount = priceAmount;
        Quantity = quantity;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // activate listing - make it public
    public void Activate()
    {
        Status = ListingStatus.Active;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // deactivate listing - hide from public
    public void Deactivate()
    {
        Status = ListingStatus.Inactive;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // reduce quantity when order is placed
    public void ReduceQuantity(int amount)
    {
        if (Status != ListingStatus.Active) throw new InvalidOperationException("Cannot reduce quantity on non-active listing");
        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero");
        if (Quantity < amount) throw new InvalidOperationException("Insufficient quantity");

        Quantity -= amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}

public enum ListingStatus
{
    Draft = 0,          // not visible to buyers
    Active = 1,         // visible and purchaseable
    Inactive = 2        // hidden from buyers
}