

using MediatR;
using Palette.Application.Interfaces;

namespace Palette.Application.Features.Orders.Commands;

// command to update order status - seller only
public record UpdateOrderStatusCommand(
    Guid OrderId,
    Guid SellerId,
    string NewStatus
) : IRequest<Unit>;

// handler for updating order status
public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // get existing order
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
            throw new InvalidOperationException("Order not found");

        // verify seller owns this order
        if (order.SellerId != request.SellerId)
            throw new UnauthorizedAccessException("Only the seller can update order status");

        // update status using domain methods
        switch (request.NewStatus.ToLower())
        {
            case "confirmed":
                order.Confirm();
                break;
            case "comlpeted":
                order.Complete();
                break;
            default:
                throw new ArgumentException($"Invalid status: {request.NewStatus}");
        }

        // save changes to database
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return Unit.Value;
    }
}