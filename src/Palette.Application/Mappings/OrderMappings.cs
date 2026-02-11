

using Palette.Application.Dtos;
using Palette.Domain.Entities;

namespace Palette.Application.Mappings;

// extension method for mapping orders to dtos
public static class OrderMappings
{
    // map orderitem to dto
    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto
        {
            Id = item.Id,
            ListingId = item.ListingId,
            TitleSnapshot = item.TitleSnapshot,
            UnitPriceSnapshot = item.UnitPriceSnapshot,
            Quantity = item.Quantity,
            LineTotal = item.LineTotal
        };
    }

    // map order to dto
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            SellerId = order.SellerId,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc,
            Items = order.Items.Select(i => i.ToDto()).ToList()
        };
    }

    // map list of orders to dtos
    public static List<OrderDto> ToDto(this List<Order> orders)
    {
        return orders.Select(o => o.ToDto()).ToList();
    }
}