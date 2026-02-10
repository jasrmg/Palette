

using Microsoft.EntityFrameworkCore;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;
using Palette.Infrastructure.Data;

namespace Palette.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PaletteDbContext _context;

    public OrderRepository(PaletteDbContext context)
    {
        _context = context;
    }

    // get order by id with items included
    public async Task<Domain.Entities.Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items) // include order items
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    // get all orders for a buyer
    public async Task<List<Order>> GetByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.BuyerId == buyerId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    // get all orders for a seller
    public async Task<List<Order>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.SellerId == sellerId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    // add new order to db
    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    // update existing order in database
    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}