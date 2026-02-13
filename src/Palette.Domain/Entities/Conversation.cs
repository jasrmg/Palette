

namespace Palette.Domain.Entities;

public class Conversation
{
    public Guid Id { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public Guid? ListingId { get; private set; }
    public DateTime LastMessageAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // messages in this conversation
    private readonly List<Message> _messages = new();
    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

    private Conversation() { }

    // create new conversation
    public Conversation(Guid buyerId, Guid sellerId, Guid? listingId = null)
    {
        if (buyerId == Guid.Empty) throw new ArgumentException("Buyer ID cannot be empty");
        if (sellerId == Guid.Empty) throw new ArgumentException("Seller ID cannot be empty");
        if (buyerId == sellerId) throw new ArgumentException("Buyer and seller cannot be the same user");


        Id = Guid.NewGuid();
        BuyerId = buyerId;
        SellerId = sellerId;
        ListingId = listingId;
        CreatedAtUtc = DateTime.UtcNow;
        LastMessageAtUtc = DateTime.UtcNow;
    }

    // add message to conversation
    public void AddMessage(Guid senderId, string body)
    {
        if (senderId == Guid.Empty) throw new ArgumentException("Sender ID cannot be empty");
        if (senderId != BuyerId && senderId != SellerId)
            throw new UnauthorizedAccessException("Only conversation participants can send messages");

        var message = new Message(Id, senderId, body);
        _messages.Add(message);
        LastMessageAtUtc = DateTime.UtcNow;
    }
}