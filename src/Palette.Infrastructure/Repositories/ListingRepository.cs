

using Microsoft.EntityFrameworkCore;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;
using Palette.Infrastructure.Data;

namespace Palette.Infrastructure.Repositories;

public class ListingRepository : IListingRepository
{
    private readonly PaletteDbContext _context;

    public ListingRepository(PaletteDbContext context)
    {
        _context = context;
    }

    // get listing by id
    public async Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    // get all lisings for a specific seller
    public async Task<List<Listing>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Where(l => l.SellerId == sellerId)
            .OrderByDescending(l => l.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    // add new listing to database
    public async Task<Listing> AddAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        _context.Listings.Add(listing);
        await _context.SaveChangesAsync(cancellationToken);
        return listing;
    }

    // update existing listing in database
    public async Task UpdateAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        _context.Listings.Update(listing);
        await _context.SaveChangesAsync(cancellationToken);
    }
}