

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Conversations.Queries;

// query to get all conversations for a user (inbox)
public record GetConversationQuery(Guid UserId) : IRequest<List<Conversation>>;

// handler for getting users conversations
public class GetConversationQueryHandler : IRequestHandler<GetConversationQuery, List<Conversation>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetConversationQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<List<Conversation>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
    {
        // get all conversations where user is buyer or seller
        return await _conversationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}

// query to get message in a specific conversation (thread)
public record GetMessagesQuery(Guid ConversationId) : IRequest<List<Message>>;

// handler for getting messages
public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<Message>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetMessagesQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<List<Message>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        // get conversation with messages
        var conversation = await _conversationRepository.GetByIdAsync
        (request.ConversationId, cancellationToken);

        if (conversation == null)
            throw new InvalidOperationException("Conversation not found");

        // return messages as list
        return conversation.Messages.ToList();
    }
}
