

using Palette.Domain.Entities;

namespace Palette.Application.Interfaces;

public interface IListingRepository
{
    // get listing by id
    Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // get all listings for a specific seller
    Task<List<Listing>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);

    // add new listing
    Task<Listing> AddAsync(Listing listing, CancellationToken cancellationToken = default);

    // update existing listing
    Task UpdateAsync(Listing listing, CancellationToken cancellationToken = default);

    // method for browse/search
    Task<(List<Listing> Items, int TotalCount)> BrowseAsync(
        string? searchTerm,
        long? minPrice,
        long? maxPrice,
        string? sortBy,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
}