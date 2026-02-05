using MediatR;
using Palette.Application.Interfaces;
using Palette.Domain.Entities;


namespace Palette.Application.Features.Auth.Commands;

public record RegisterUserCommand(
    string Email,           // users email address will be the username
    string Password,        // plain text password will be hashed
    string FirstName,
    string LastName
) : IRequest<Guid>;         // returns the new user ID on success

// handler contains the business logic for user registration
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // check if user already exists with this email
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser != null)
            throw new InvalidOperationException("Registration failed. Please try again or contact support.");

        // hash the password for secure storage
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // create new user domain entity
        var user = new User(request.Email, hashedPassword, request.FirstName, request.LastName);

        // save to db through repository
        await _userRepository.AddAsync(user, cancellationToken);

        return user.Id; // return user id
    }
}