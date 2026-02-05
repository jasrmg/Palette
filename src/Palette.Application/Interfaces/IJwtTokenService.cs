

namespace Palette.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email);
}