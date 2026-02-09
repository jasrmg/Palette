

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palette.Application.Dtos;
using Palette.Application.Features.Listings.Commands;
using Palette.Application.Features.Listings.Queries;
using Palette.Domain.Entities; // will be replaced with app.dtos

namespace Palette.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IMediator _mediator; // mediatr for sending commands
    public ListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/listings - create new listing
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateListing([FromBody] CreateListingCommand command)
    {
        try
        {
            // send command to handler, get new listing id
            var listingId = await _mediator.Send(command);
            return Ok(listingId);
        }
        catch (ArgumentException ex)
        {
            // validation error from domain
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/listings/{id} - update existing listing
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateListing(Guid id, [FromBody] UpdateListingCommand command)
    {
        try
        {
            // ensure route id matches command id
            if (id != command.ListingId)
                return BadRequest(new { message = "Listing ID mistmatch" });

            // send command to handler
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // listing not found
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            // not the owner
            return Forbid();
        }
    }

    // DELETE /api.listings/{id} - deactivate listing
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeactivateListing(Guid id, [FromQuery] Guid sellerId)
    {
        try
        {
            // send command to handler
            var command = new DeactivateListingCommand(id, sellerId);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // listing not found
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            // not the owner
            return Forbid();
        }
    }

    // GET /api/listings/{id} - get listing by id
    [HttpGet("{id}")]
    public async Task<ActionResult<ListingDto>> GetListingById(Guid id)
    {
        var query = new GetListingByIdQuery(id);
        var listing = await _mediator.Send(query);

        if (listing == null)
            return NotFound(new { message = "Listing not found" });

        return Ok(listing);
    }

    // GET /api/listings/my?sellerId={sellerId} - get sellers listings
    [HttpGet("my")]
    public async Task<ActionResult<List<ListingDto>>> GetMyListings([FromQuery] Guid sellerId)
    {
        var query = new GetMyListingsQuery(sellerId);
        var listings = await _mediator.Send(query);
        return Ok(listings);
    }
}