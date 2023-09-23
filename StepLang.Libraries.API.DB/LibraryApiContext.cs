using Microsoft.EntityFrameworkCore;
using StepLang.Libraries.API.DB.Entities;

namespace StepLang.Libraries.API.DB;

public class LibraryApiContext : DbContext
{
    public LibraryApiContext(DbContextOptions<LibraryApiContext> options) : base(options)
    {
    }

    public DbSet<Library> Libraries { get; set; } = null!;

    public DbSet<LibraryVersion> LibraryVersions { get; set; } = null!;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Library>()
            .HasOne<LibraryVersion>(l => l.LatestVersion)
            .WithOne(v => v.Library);

        modelBuilder.Entity<Library>()
            .HasMany<LibraryVersion>(l => l.Versions);

        modelBuilder.Entity<LibraryVersion>()
            .HasMany<Library>(v => v.Dependencies);

        modelBuilder.Entity<LibraryVersion>()
            .HasMany<Library>(v => v.Dependents);
    }
}
