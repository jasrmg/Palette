

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Listings.Queries;

// query to get all listings for current seller
public record GetMyListingsQuery(Guid SellerId) : IRequest<List<Listing>>;

// handler 
public class GetMyListingsQueryHandler : IRequestHandler<GetMyListingsQuery, List<Listing>>
{
    private readonly IListingRepository _listingRepository;
    public GetMyListingsQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<List<Listing>> Handle(GetMyListingsQuery request, CancellationToken cancellationToken)
    {
        return await _listingRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);
    }
}