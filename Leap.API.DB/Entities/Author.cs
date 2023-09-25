namespace Leap.API.DB.Entities;

public class Author
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public ICollection<Library> Libraries { get; set; } = null!;

    /// <inheritdoc />
    public override string ToString() => $"Author(Id={Id}, Username={Username})";
}