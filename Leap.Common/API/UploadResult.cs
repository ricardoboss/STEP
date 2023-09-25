namespace Leap.Common.API;

public record UploadResult(string Code, string Message, BriefLibraryVersion? Version = null)
{
    public static UploadResult VersionInvalid(string version) => new("version_invalid", $"Version '{version}' is invalid.");

    public static UploadResult VersionAlreadyExists(string version) => new("version_already_exists", $"Version '{version}' already exists.");

    public static UploadResult VersionMustBeNewer(string version, string latest) => new("version_must_be_newer", $"Version '{version}' must be newer than the latest version '{latest}'.");

    public static UploadResult LengthRequired() => new("length_required", $"Content-Length header is required.");

    public static UploadResult TooLarge(long size, long max) => new("too_large", $"Payload is too large ({size} bytes). Maximum allowed size is {max} bytes.");

    public static UploadResult LibraryFileMissing() => new("library_file_missing", "Library file is missing.");

    public static UploadResult Success(string name, string version, BriefLibraryVersion brief) => new("success", $"Version '{version}' of {name} uploaded successfully.", brief);

    public static UploadResult Unauthorized() => new("unauthorized", "Unauthorized.");

    public static UploadResult OwnerMismatch() => new("owner_mismatch", "The current user is not the owner of the library.");
}
