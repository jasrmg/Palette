

using MediatR;
using Palette.Application.Dtos;
using Palette.Application.Interfaces;
using Palette.Application.Mappings;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Listings.Queries;

// query to get all listings for current seller
public record GetMyListingsQuery(Guid SellerId) : IRequest<List<ListingDto>>;

// handler for getting selers listings
public class GetMyListingsQueryHandler : IRequestHandler<GetMyListingsQuery, List<ListingDto>>
{
    private readonly IListingRepository _listingRepository;
    public GetMyListingsQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<List<ListingDto>> Handle(GetMyListingsQuery request, CancellationToken cancellationToken)
    {
        // get domain entities from repo
        var listings = await _listingRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);

        // map list of domain entities to list of dtos
        return listings.ToDto();
    }
}