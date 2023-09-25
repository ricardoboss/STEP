using Leap.API.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Leap.API.DB;

public class LeapApiDbContext : DbContext
{
    public LeapApiDbContext(DbContextOptions<LeapApiDbContext> options) : base(options)
    {
    }

    public DbSet<Library> Libraries { get; set; } = null!;

    public DbSet<LibraryVersion> LibraryVersions { get; set; } = null!;

    public DbSet<Author> Authors { get; set; } = null!;

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

        modelBuilder.Entity<Library>()
            .HasMany<Author>(l => l.Maintainers)
            .WithMany(a => a.Libraries);

        modelBuilder.Entity<Library>()
            .HasIndex(l => new
            {
                l.Author,
                l.Name,
            })
            .IsUnique();

        modelBuilder.Entity<LibraryVersion>()
            .HasMany<Library>(v => v.Dependencies)
            .WithMany(l => l.Versions)
            .UsingEntity<LibraryVersionDependency>(
                j => j.HasOne(d => d.Dependency).WithMany(),
                j => j.HasOne(d => d.Version).WithMany()
            );
    }
}