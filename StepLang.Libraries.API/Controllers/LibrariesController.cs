using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Semver;
using StepLang.Libraries.API.DB;
using StepLang.Libraries.API.DB.Entities;
using StepLang.Libraries.API.Extensions;
using StepLang.Libraries.API.Interfaces;
using StepLang.Libraries.API.Services;

namespace StepLang.Libraries.API.Controllers;

[ApiController]
[Route("[controller]/{name}")]
public class LibrariesController : ControllerBase
{
    private readonly LibraryApiContext context;
    private readonly ILibraryStorage storage;

    public LibrariesController(LibraryApiContext context, ILibraryStorage storage)
    {
        this.context = context;
        this.storage = storage;
    }

    private string GetDownloadUrl(string name, string version) => Url.ActionLink("Download", "Libraries", new
    {
        name,
        version,
    }) ?? throw new("Failed to generate download URL.");

    [HttpGet("{version?}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BriefLibraryVersion), StatusCodes.Status200OK)]
    public async Task<BriefLibraryVersion?> Get(string name, string? version, CancellationToken cancellationToken = default)
    {
        var library = await context.Libraries
            .Include(library => library.LatestVersion)
            .ThenInclude(v => v!.Dependencies)
            .Include(library => library.Versions)
            .ThenInclude(v => v.Dependencies)
            .FirstOrDefaultAsync(l => l.Name == name, cancellationToken);

        if (library is null)
            return null;

        LibraryVersion? libraryVersion;
        if (version is null)
        {
            if (library.LatestVersion is null)
                return null;

            libraryVersion = library.LatestVersion;
        }
        else if (SemVersion.TryParse(version, SemVersionStyles.Strict, out var semVersion))
        {
            libraryVersion = library.Versions.FirstOrDefault(v => v.Version == semVersion);
        }
        else if (SemVersionRange.TryParse(version, out var semVersionRange))
        {
            libraryVersion = library.Versions
                .Where(v => semVersionRange.Contains(v.Version))
                .MaxBy(v => SemVersion.Parse(v.Version, SemVersionStyles.Strict));
        }
        else
        {
            // bad version, TODO: return error
            return null;
        }

        if (libraryVersion is null)
            return null;

        var downloadUrl = GetDownloadUrl(name, libraryVersion.Version);

        return libraryVersion.ToBriefDto(downloadUrl);
    }

    [HttpGet("{version}/download")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Download(string name, string version, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!await storage.ExistsAsync(name, version, cancellationToken))
                return NotFound();

            await storage.UpdateMetadataAsync(name, version, metadata =>
            {
                metadata.TryAdd("Downloads", 0);
                metadata["Downloads"]++;
            }, cancellationToken);

            await using var stream = await storage.OpenReadAsync(name, version, cancellationToken);

            return File(stream, "application/zip", $"{name}-{version}.zip");
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpPost("{version}")]
    [ProducesResponseType(typeof(UploadResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(UploadResult), StatusCodes.Status411LengthRequired)]
    [ProducesResponseType(typeof(UploadResult), StatusCodes.Status413PayloadTooLarge)]
    [ProducesResponseType(typeof(UploadResult), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(UploadResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(string name, string version, CancellationToken cancellationToken = default)
    {
        if (!User.HasClaim(c => c.Type == TokenGenerator.IdClaim))
            return Unauthorized(UploadResult.Unauthorized());

        var authorIdStr = User.FindFirstValue(TokenGenerator.IdClaim);
        if (authorIdStr is null || !Guid.TryParse(authorIdStr, out var authorId))
            return Unauthorized(UploadResult.Unauthorized());

        var author = await context.Authors
            .Include(author => author.Libraries)
            .FirstOrDefaultAsync(a => a.Id == authorId, cancellationToken);

        if (author is null)
            return Unauthorized(UploadResult.Unauthorized());

        var library = await context.Libraries
            .Include(l => l.LatestVersion)
            .Include(l => l.Versions)
            .FirstOrDefaultAsync(l => l.Name == name, cancellationToken);

        if (library is null)
        {
            library = new()
            {
                Name = name,
                Author = author,
                Versions = new List<LibraryVersion>(),
            };

            await context.Libraries.AddAsync(library, cancellationToken);

            author.Libraries.Add(library);
        }
        else
        {
            // check if the current author owns the library
            if (author.Libraries.All(l => l.Name != name))
                return StatusCode(StatusCodes.Status403Forbidden, UploadResult.OwnerMismatch());
        }

        SemVersion newVersionSemVer;
        try
        {
            newVersionSemVer = SemVersion.Parse(version, SemVersionStyles.Strict);
        }
        catch (FormatException)
        {
            return UnprocessableEntity(UploadResult.VersionInvalid(version));
        }

        var existingVersion = library.Versions.FirstOrDefault(v => v.Version == version);
        if (existingVersion is not null)
            return UnprocessableEntity(UploadResult.VersionAlreadyExists(version));

        var latestVersion = library.LatestVersion;
        if (latestVersion is not null)
        {
            var latestVersionSemVer = SemVersion.Parse(latestVersion.Version, SemVersionStyles.Strict);

            if (newVersionSemVer.ComparePrecedenceTo(latestVersionSemVer) != 1)
                return UnprocessableEntity(UploadResult.VersionMustBeNewer(version, latestVersion.Version));
        }

        const long maxUploadSize = 100_000_000; // 100 MB

        var size = Request.Headers.ContentLength;
        switch (size)
        {
            case null:
                return StatusCode(StatusCodes.Status411LengthRequired, UploadResult.LengthRequired());
            case > maxUploadSize:
                return StatusCode(StatusCodes.Status413PayloadTooLarge, UploadResult.TooLarge(size.Value, maxUploadSize));
        }

        await using var targetStream = storage.OpenWrite(name, version, cancellationToken);

        await Request.Body.CopyToAsync(targetStream, cancellationToken);

        var libraryVersion = new LibraryVersion
        {
            Version = version,
            Library = library,
            Dependencies = new List<DB.Entities.Library>(),
        };

        await context.LibraryVersions.AddAsync(libraryVersion, cancellationToken);

        library.LatestVersion = libraryVersion;

        await context.SaveChangesAsync(cancellationToken);

        var downloadUrl = GetDownloadUrl(name, version);

        return Ok(UploadResult.Success(name, version, libraryVersion.ToBriefDto(downloadUrl)));
    }
}