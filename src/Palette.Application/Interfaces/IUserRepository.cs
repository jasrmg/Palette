using Palette.Domain.Entities;

namespace Palette.Application.Interfaces;

// repository abstraction for user data operations
public interface IUserRepository
{
    // check if user exists by email
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    // add user to database
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
}