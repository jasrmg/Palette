using MediatR;
using Palette.Application.Dtos;
using Palette.Application.Interfaces;
using Palette.Application.Mappings;

namespace Palette.Application.Features.Orders.Queries;

// query to get all orders for a buyer
public record GetMyOrdersQuery(Guid BuyerId) : IRequest<List<OrderDto>>;


// handler for getting buyers orders
public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        // get orders from repository
        var orders = await _orderRepository.GetByBuyerIdAsync(request.BuyerId, cancellationToken);

        // map to dtos
        return orders.ToDto();
    }
}

// query to get all orders for a seller
public record GetSellerOrdersQuery(Guid SellerId) : IRequest<List<OrderDto>>;

// handler for getrting sellers orders
public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSellerOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
    {
        // get orders from repository
        var orders = await _orderRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);

        // map to dtos
        return orders.ToDto();
    }
}