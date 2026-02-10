

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

    // browse listings with filters, sorting and pagination
    public async Task<(List<Listing> Items, int TotalCount)> BrowseAsync(
        string? searchTerm,
        long? minPrice,
        long? maxPrice,
        string? sortBy,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        // start with base query - only active listing
        var query = _context.Listings
            .Where(l => l.Status == ListingStatus.Active);

        // apply search filter (title or description contains search term)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(l =>
                l.Title.Contains(searchTerm) ||
                l.Description.Contains(searchTerm));
        }

        // apply price filters
        if (minPrice.HasValue)
        {
            query = query.Where(l => l.PriceAmount >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(l => l.PriceAmount <= maxPrice.Value);
        }

        // get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // apply sorting
        query = sortBy?.ToLower() switch
        {
            "price" => query.OrderBy(l => l.PriceAmount),
            "price_desc" => query.OrderByDescending(l => l.PriceAmount),
            _ => query.OrderByDescending(l => l.CreatedAtUtc) // default: newest first
        };

        // apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }


}