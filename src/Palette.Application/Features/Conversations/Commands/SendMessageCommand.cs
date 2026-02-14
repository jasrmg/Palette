

using MediatR;
using Palette.Application.Interfaces;

namespace Palette.Application.Features.Conversations.Commands;

// command to send message in existing conversation
public record SendMessageCommand(
    Guid ConversationId,
    Guid SenderId,
    string Body
) : IRequest<Guid>;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Guid>
{
    private readonly IConversationRepository _conversationRepository;

    public SendMessageCommandHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // get existing conversation
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);

        if (conversation == null)
            throw new InvalidOperationException("Conversation not found");

        // add message using domain method (enforses sender is participant)
        conversation.AddMessage(request.SenderId, request.Body);

        // update conversation - LastMessageAtUtc update
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        // return new message ID
        return conversation.Messages.Last().Id;
    }
}