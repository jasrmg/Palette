

using MediatR;
using Palette.Application.Dtos;
using Palette.Application.Interfaces;
using Palette.Application.Mappings;

namespace Palette.Application.Features.Listings.Queries;

public record GetListingByIdQuery(Guid ListingId) : IRequest<ListingDto?>;

// handler for getting listing by id
public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, ListingDto?>
{
    private readonly IListingRepository _listingRepository;

    public GetListingByIdQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<ListingDto?> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        // get domain entity from repo
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);

        // return null if not found
        if (listing == null)
            return null;

        // map domain entity to dto
        return listing.ToDto();
    }
}