

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Listings.Commands;

public record CreateListingCommand(
    Guid SellerId,
    string Title,
    string Description,
    long PriceAmount,
    int Quantity
) : IRequest<Guid>;

// hanler for creating listings
public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingRepository _listingRepository;

    public CreateListingCommandHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        // create new listing domain entity - start as draft
        var listing = new Listing(
            request.SellerId,
            request.Title,
            request.Description,
            request.PriceAmount,
            request.Quantity
        );

        // save to db through repository
        await _listingRepository.AddAsync(listing, cancellationToken);

        return listing.Id;
    }
}