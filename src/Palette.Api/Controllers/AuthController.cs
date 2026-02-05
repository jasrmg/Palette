

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palette.Application.Features.Auth.Commands;

namespace Palette.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator; // Mediatr for sending commands

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/auth/register - Register new user
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            // send command to handler, get new user ID
            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(Register), new { id = userId }, userId);
        }
        catch (InvalidOperationException ex)
        {
            // user already exists
            return Conflict(new { message = ex.Message });
        }
    }

    // POST /api/auth/login - Login user
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            // sendc command to handler, get JWT token
            var token = await _mediator.Send(command);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {
            // invalid credentials
            return Unauthorized(new { message = ex.Message });
        }
    }


}