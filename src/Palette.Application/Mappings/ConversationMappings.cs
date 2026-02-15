

using Palette.Application.Dtos;
using Palette.Domain.Entities;

namespace Palette.Application.Mappings;

// extension methods for mapping conversations to dtos
public static class ConversationMappings
{
    // map message to dto
    public static MessageDto ToDto(this Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            Body = message.Body,
            SentAtUtc = message.SentAtUtc
        };
    }

    // map conversation to dto
    public static ConversationDto ToDto(this Conversation conversation)
    {
        return new ConversationDto
        {
            Id = conversation.Id,
            BuyerId = conversation.BuyerId,
            SellerId = conversation.SellerId,
            ListingId = conversation.ListingId,
            LastMessageAtUtc = conversation.LastMessageAtUtc,
            CreatedAtUtc = conversation.CreatedAtUtc,
            Messages = conversation.Messages.Select(m => m.ToDto()).ToList()
        };
    }

    // map list of conversations to dtos
    public static List<ConversationDto> ToDto(this List<Conversation> conversations)
    {
        return conversations.Select(c => c.ToDto()).ToList();
    }
}