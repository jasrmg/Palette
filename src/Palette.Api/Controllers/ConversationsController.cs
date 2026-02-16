

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palette.Application.Dtos;
using Palette.Application.Features.Conversations.Commands;
using Palette.Application.Features.Conversations.Queries;

namespace Palette.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/conversations - create new conversations
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateConversation([FromBody] CreateConversationCommand command)
    {
        try
        {
            var conversationId = await _mediator.Send(command);
            return Ok(conversationId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/conversations?userId={userId} - get users conversation (inbox)
    [HttpGet]
    public async Task<ActionResult<List<ConversationDto>>> GetConversations()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var query = new GetConversationQuery(userId);
        var conversations = await _mediator.Send(query);
        return Ok(conversations);
    }
    // GET /api/conversations/{id}/messages - get messages in conversation
    [HttpGet("{id}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(Guid id)
    {
        try
        {
            var query = new GetMessagesQuery(id);
            var messages = await _mediator.Send(query);
            return Ok(messages);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST /api/conversations/{id}/messages - send message
    [HttpPost("{id}/messages")]
    public async Task<ActionResult<Guid>> SendMessage(Guid id, [FromBody] SendMessageCommand command)
    {
        try
        {
            if (id != command.ConversationId)
                return BadRequest(new { message = "Conversation ID mismatch" });

            var messageId = await _mediator.Send(command);
            return Ok(messageId);
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