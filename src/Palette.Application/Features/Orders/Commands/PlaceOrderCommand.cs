

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Orders.Commands;

// command to place a new order
public record PlaceOrderCommand(
    Guid BuyerId,
    Guid SellerId,
    List<OrderItemRequest> Items
) : IRequest<Guid>;

// request for each item in the order
public record OrderItemRequest(
    Guid ListingId,
    int Quantity
);

// handler for placing orders
public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IListingRepository _listingRepository;

    public PlaceOrderCommandHandler(IOrderRepository orderRepository, IListingRepository listingRepository)
    {
        _orderRepository = orderRepository;
        _listingRepository = listingRepository;
    }

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var orderItems = new List<OrderItem>();

        // process each item in the order
        foreach (var itemRequest in request.Items)
        {
            // get listing to capture price snapshot
            var listing = await _listingRepository.GetByIdAsync(itemRequest.ListingId, cancellationToken);

            if (listing == null)
                throw new InvalidOperationException($"Listing {itemRequest.ListingId} not found");

            if (listing.Status != ListingStatus.Active)
                throw new InvalidOperationException($"Listing {listing.Title} is not available");

            if (listing.Quantity < itemRequest.Quantity)
                throw new InvalidOperationException($"Insufficient quantity for {listing.Title}");

            // create order item with price snapshot
            var orderItem = new OrderItem(
                listing.Id,
                listing.Title,
                listing.PriceAmount,
                itemRequest.Quantity
            );

            orderItems.Add(orderItem);

            // reduce listing quantity
            listing.ReduceQuantity(itemRequest.Quantity);
            await _listingRepository.UpdateAsync(listing, cancellationToken);
        }

        // create order with order items
        var order = new Order(request.BuyerId, request.SellerId, orderItems);
        // save order to db
        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}