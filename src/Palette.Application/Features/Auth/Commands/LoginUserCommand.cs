using MediatR;
using Palette.Application.Interfaces;

namespace Palette.Application.Features.Auth.Commands;

// command represents the request to login a user
public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<string>; // returns JWT token on successful login

// handler contains the busines logic for user auth
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository; // repository abstraction
    private readonly IPasswordHasher _passwordHasher; // password verification service
    private readonly IJwtTokenService _jwtTokenService; // jwt token generation service

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        string Error = "Invalid email or password";
        if (user == null)
            throw new UnauthorizedAccessException(Error);

        // verify password agianst stored hash
        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException(Error);

        // check if user account is active
        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated");

        // generate JWT token for authenticated user
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email);

        return token; // return jwt token
    }

}