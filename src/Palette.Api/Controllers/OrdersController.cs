

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palette.Application.Dtos;
using Palette.Application.Features.Orders.Commands;
using Palette.Application.Features.Orders.Queries;

namespace Palette.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/orders - place new order
    [HttpPost]
    public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        try
        {
            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/orders/my?buyerId={buyerId} - get buyers orders
    [HttpGet("my")]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        // get buyerid from jwt token
        var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var query = new GetMyOrdersQuery(buyerId);
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    // GET /api/orders/seller?sellerId={sellerId} - get sellers orders
    [HttpGet("seller")]
    public async Task<ActionResult<List<OrderDto>>> GetSellerOrders()
    {
        // get seller id from jwt token
        var sellerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetSellerOrdersQuery(sellerId);
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    // PATCH /api/orders/{id}/status - update order status
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusCommand command)
    {
        try
        {
            if (id != command.OrderId)
                return BadRequest(new { message = "Order ID mismatch" });

            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}