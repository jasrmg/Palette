

using MediatR;
using Palette.Application.Dtos;
using Palette.Application.Interfaces;
using Palette.Application.Mappings;

namespace Palette.Application.Features.Listings.Queries;

// query to browse public listings with filters and pagination
public record BrowseListingsQuery(
    string? SearchTerm,
    long? MinPrice,
    long? MaxPrice,
    string? SortBy,
    int Page = 1,
    int PageSize = 10
) : IRequest<BrowseListingsResponse>;

// response with pagination info
public class BrowseListingsResponse
{
    public List<ListingDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

// handler for browsing listings
public class BrowseListingsQueryHandler : IRequestHandler<BrowseListingsQuery, BrowseListingsResponse>
{
    private readonly IListingRepository _listingRepository;

    public BrowseListingsQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<BrowseListingsResponse> Handle(BrowseListingsQuery request, CancellationToken cancellationToken)
    {
        // get listings from repository with filters
        var (listings, totalCount) = await _listingRepository.BrowseAsync(
            request.SearchTerm,
            request.MinPrice,
            request.MaxPrice,
            request.SortBy,
            request.Page,
            request.PageSize,
            cancellationToken
        );

        // map domain entities to dtos
        var listingDtos = listings.ToDto();

        // calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);


        // return response with pagination info
        return new BrowseListingsResponse
        {
            Items = listingDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };

    }
}