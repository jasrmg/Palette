using Microsoft.EntityFrameworkCore;
using Palette.Domain.Entities;

namespace Palette.Infrastructure.Data;

public class PaletteDbContext : DbContext
{
    public PaletteDbContext(DbContextOptions<PaletteDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // amazonq-ignore-next-line
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

        });
    }
}