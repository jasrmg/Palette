using Palette.Application.Dtos;
using Palette.Domain.Entities;

namespace Palette.Application.Mappings;


// extension methods for mapping between domain entitites and dtos
public static class ListingMappings
{

    // map domain entitty to dto
    public static ListingDto ToDto(this Listing listing)
    {
        return new ListingDto
        {
            Id = listing.Id,
            SellerId = listing.SellerId,
            Title = listing.Title,
            Description = listing.Description,
            PriceAmount = listing.PriceAmount,
            Currency = listing.Currency,
            Quantity = listing.Quantity,
            Status = listing.Status.ToString(), // convert enum to str
            CreatedAtUtc = listing.CreatedAtUtc,
            UpdatedAtUtc = listing.UpdatedAtUtc
        };
    }

    // map list of domain entities to list of dtos
    public static List<ListingDto> ToDto(this List<Listing> listings)
    {
        return listings.Select(l => l.ToDto()).ToList();
    }
}