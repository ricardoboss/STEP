using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepLang.Libraries.API.DB;
using StepLang.Libraries.API.Extensions;

namespace StepLang.Libraries.API.Controllers;

[ApiController]
[Route("[controller]/{name}")]
public class LibrariesController : ControllerBase
{
    private readonly LibraryApiContext context;

    public LibrariesController(LibraryApiContext context)
    {
        this.context = context;
    }

    [HttpGet("{version?}")]
    public async Task<BriefLibraryDto?> Get(string name, string? version, CancellationToken cancellationToken = default)
    {
        var library = await context.Libraries
            .Include(library => library.LatestVersion)
            .Include(library => library.Versions)
            .ThenInclude(v => v.Dependencies)
            .FirstOrDefaultAsync(l => l.Name == name, cancellationToken);

        if (library is null)
            return null;

        if (version is null)
            return library.LatestVersion?.ToBriefDto();

        var libraryVersion = library.Versions.First(v => v.Version == version);

        return libraryVersion.ToBriefDto();
    }
}