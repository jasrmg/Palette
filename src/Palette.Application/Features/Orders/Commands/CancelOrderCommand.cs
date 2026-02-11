

using MediatR;
using Palette.Application.Features.Orders.Commands;
using Palette.Application.Interfaces;

public record CancelOrderCommand(
    Guid OrderId,
    Guid UserId
) : IRequest<Unit>;

public class CancelOrderComamndsHandler : IRequestHandler<CancelOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    public CancelOrderComamndsHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        // get existing order
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
            throw new InvalidOperationException("Order not found");

        // verify user is buyer or seller
        if (order.BuyerId != request.UserId && order.SellerId != request.UserId)
            throw new UnauthorizedAccessException("You can only cancel your own orders");

        // cancel order using domain method
        order.Cancel();

        // save changes to database
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return Unit.Value;
    }
}