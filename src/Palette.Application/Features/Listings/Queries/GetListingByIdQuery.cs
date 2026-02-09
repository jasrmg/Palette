

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Listings.Queries;

public record GetListingByIdQuery(Guid ListingId) : IRequest<Listing?>;

// handler for getting listing by id
public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, Listing?>
{
    private readonly IListingRepository _listingRepository;

    public GetListingByIdQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Listing?> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
    }
}