

using Palette.Domain.Entities;

namespace Palette.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Order>> GetByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken);

    Task<List<Order>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken);

    Task<Order> AddAsync(Order order, CancellationToken cancellationToken);

    Task UpdateAsync(Order order, CancellationToken cancellationToken);
}