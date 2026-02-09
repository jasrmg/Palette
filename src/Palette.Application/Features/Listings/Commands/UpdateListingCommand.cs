

using MediatR;
using Palette.Application.Interfaces;

namespace Palette.Application.Features.Listings.Commands;

public record UpdateListingCommand(
    Guid ListingId,
    Guid SellerId,
    string Title,
    string Description,
    long PriceAmount,
    int Quantity
) : IRequest<Unit>; // returns nothing on success

// handler for updating listings
public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, Unit>
{
    private readonly IListingRepository _listingRepository;

    public UpdateListingCommandHandler(IListingRepository listingRepository)
    {

        _listingRepository = listingRepository;
    }

    public async Task<Unit> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        // get existing listing
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);

        if (listing == null)
            throw new InvalidOperationException("Listing not found");

        // verify seller owns the listing
        if (listing.SellerId != request.SellerId)
            throw new UnauthorizedAccessException("You can only update your own listings");

        // update listing using domain method
        listing.Update(
            request.Title,
            request.Description,
            request.PriceAmount,
            request.Quantity
        );

        // save changes to database
        await _listingRepository.UpdateAsync(listing, cancellationToken);

        return Unit.Value;

    }
}