

namespace Palette.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAtUtc { get; private set; }

    private User() { } // EF Core

    public User(string email, string passwordHash, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash cannot be empty");
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be empty");

        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be empty");

        FirstName = firstName;
        LastName = lastName;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

}