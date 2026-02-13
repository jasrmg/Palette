

using Palette.Domain.Entities;

namespace Palette.Application.Interfaces;

public interface IConversationRepository
{
    // get conversation by id with messages
    Task<Conversation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // get all conversations for a user - buyer or seller
    Task<List<Conversation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    // add new conversation
    Task<Conversation> AddAsync(Conversation conversation, CancellationToken cancellationToken);

    // update conversation for lastmessageatutc
    Task UpdateAsync(Conversation conversation, CancellationToken cancellationToken);
}