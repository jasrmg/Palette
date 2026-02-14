
using Microsoft.EntityFrameworkCore;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;
using Palette.Infrastructure.Data;

namespace Palette.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly PaletteDbContext _context;

    public ConversationRepository(PaletteDbContext context)
    {
        _context = context;
    }

    // get conversation by id with all messages included
    public async Task<Conversation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .Include(c => c.Messages) // eager load messages
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    // get all conversations where user is buyer or seller
    public async Task<List<Conversation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .Include(c => c.Messages) // include messages for each conversation
            .Where(c => c.BuyerId == userId || c.SellerId == userId)
            .OrderByDescending(c => c.LastMessageAtUtc) // newest conversations first
            .ToListAsync(cancellationToken);
    }

    // add new conversation to database
    public async Task<Conversation> AddAsync(Conversation conversation, CancellationToken cancellationToken = default)
    {
        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);
        return conversation;
    }

    // update conversation - when new message is added
    public async Task UpdateAsync(Conversation conversation, CancellationToken cancellationToken = default)
    {
        _context.Conversations.Update(conversation);
        await _context.SaveChangesAsync(cancellationToken);
    }
}