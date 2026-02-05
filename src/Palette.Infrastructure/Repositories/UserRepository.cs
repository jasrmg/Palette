

using Microsoft.EntityFrameworkCore;
using Palette.Application.Interfaces;
using Palette.Infrastructure.Data;
using Palette.Domain.Entities;

namespace Palette.Infrastructure.Repositories;


public class UserRepository : IUserRepository
{
    private readonly PaletteDbContext _context;

    public UserRepository(PaletteDbContext context)
    {
        _context = context;
    }

    // find user by email address
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    // add new user and save to database
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }
}