

using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;

namespace Palette.Application.Features.Conversations.Commands;

// command to create new conversation between buyer and seller
public record CreateConversationCommand(
    Guid BuyerId,
    Guid SellerId,
    Guid? ListingId = null
) : IRequest<Guid>;

// handler for creating conversations
public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, Guid>
{
    private readonly IConversationRepository _conversationRepository;

    public CreateConversationCommandHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<Guid> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        // create new conversation domain entity
        var conversation = new Conversation(
            request.BuyerId,
            request.SellerId,
            request.ListingId
        );

        // save to db
        await _conversationRepository.AddAsync(conversation, cancellationToken);

        return conversation.Id;
    }


}