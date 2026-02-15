

using MediatR;
using Palette.Application.Dtos;
using Palette.Application.Interfaces;
using Palette.Application.Mappings;

namespace Palette.Application.Features.Conversations.Queries;

// query to get all conversations for a user (inbox)
public record GetConversationQuery(Guid UserId) : IRequest<List<ConversationDto>>;

// handler for getting users conversations
public class GetConversationQueryHandler : IRequestHandler<GetConversationQuery, List<ConversationDto>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetConversationQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<List<ConversationDto>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
    {
        // get all conversations where user is buyer or seller
        var conversations = await _conversationRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        // map to dtos
        return conversations.ToDto();
    }
}

// query to get message in a specific conversation (thread)
public record GetMessagesQuery(Guid ConversationId) : IRequest<List<MessageDto>>;

// handler for getting messages
public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<MessageDto>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetMessagesQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<List<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        // get conversation with messages
        var conversation = await _conversationRepository.GetByIdAsync
        (request.ConversationId, cancellationToken);

        if (conversation == null)
            throw new InvalidOperationException("Conversation not found");

        // return messages as list
        return conversation.Messages.Select(m => m.ToDto()).ToList();
    }
}
