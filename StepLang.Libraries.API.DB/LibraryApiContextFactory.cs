using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StepLang.Libraries.API.DB;

public class LibraryApiContextFactory : IDesignTimeDbContextFactory<LibraryApiContext>
{
    public LibraryApiContext CreateDbContext(string [] args)
    {
        var builder = new DbContextOptionsBuilder<LibraryApiContext>();

        builder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=step;User Id=postgres;Password=postgres;");

        return new(builder.Options);
    }
}