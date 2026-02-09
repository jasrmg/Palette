

using MediatR;
using Palette.Application.Interfaces;

namespace Palette.Application.Features.Listings.Commands;

public record DeactivateListingCommand(
    Guid ListingId,
    Guid SellerId
) : IRequest<Unit>;

// handler for deactivating listings
public class DeactivateListingCommandHandler : IRequestHandler<DeactivateListingCommand, Unit>
{
    private readonly IListingRepository _listingRepository;

    public DeactivateListingCommandHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Unit> Handle(DeactivateListingCommand request, CancellationToken cancellationToken)
    {
        // get existing listing
        var listing = await _listingRepository.GetByIdAsync(request.ListingId);

        if (listing == null)
            throw new InvalidOperationException("Listing not found");

        // verify seller owns the listing
        if (listing.SellerId != request.SellerId)
            throw new UnauthorizedAccessException("You can only deactivate your own listings");

        // deactivate using the domain method
        listing.Deactivate();

        // save changes to db
        await _listingRepository.UpdateAsync(listing, cancellationToken);

        return Unit.Value;
    }
}